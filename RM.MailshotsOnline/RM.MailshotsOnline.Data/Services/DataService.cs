﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using HC.RM.Common;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Data.Services
{
    public class DataService : IDataService
    {
        private readonly BlobStorageHelper _blobStorage =
            new BlobStorageHelper(ConfigHelper.PrivateStorageConnectionString,
                                  ConfigHelper.PrivateDistributionListBlobStorageContainer);

        private readonly ILogger _logger;
        private readonly StorageContext _context;

        private string _className = "DataService";
        private readonly string _elementDistributionList = "distributionList";
        private readonly string _elementErrors = "errors";
        private readonly string _elementInvalid = "invalid";
        private readonly string _elementDuplicates = "duplicates";
        private readonly string _attributeListName = "listName";
        private readonly string _attributeCount = "count";

        public DataService(ILogger logger) : this(logger, new StorageContext())
        { }

        public DataService(ILogger logger, StorageContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IDistributionList GetDistributionListForUser(int userId, Guid distributionListId)
        {
            return GetDistributionListsForUser(userId).SingleOrDefault(d => d.DistributionListId == distributionListId);
        }

        public IEnumerable<IDistributionList> GetDistributionListsForUser(int userId)
        {
            return GetDistributionLists(d => d.UserId == userId).OrderBy(d=> d.Name);
        }

        public bool ListNameIsAlreadyInUse(int userId, string listName)
        {
            return GetDistributionListsForUser(userId).Any(d => string.Equals(d.Name, listName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<IDistributionList> GetDistributionLists(Func<IDistributionList, bool> filter)
        {
            return _context.DistributionLists.Where(filter);
        }

        public IDistributionList SaveDistributionList(IDistributionList distributionList)
        {
            distributionList.UpdatedDate = DateTime.UtcNow;
            if (distributionList.DistributionListId == Guid.Empty)
            {
                _context.DistributionLists.Add((DistributionList)distributionList);
            }

            _context.SaveChanges();

            return distributionList;
        }

        public void DeleteDistributionList(IDistributionList distributionList)
        {
            _context.DistributionLists.Remove((DistributionList)distributionList);

            _context.SaveChanges();
        }

        public IDistributionList CreateDistributionList(IMember member, string listName, Enums.DistributionListState listState, byte[] bytes, string contentType, Enums.DistributionListFileType fileType)
        {
            string listNameAsFileName = convertToFileName(listName, contentType, fileType);
            var uploadedListName = $"{member.Id}/{listNameAsFileName}";

            try
            {
                _blobStorage.StoreBytes(bytes, uploadedListName, contentType);
            }
            catch (Exception ex)
            {
                _logger.Exception(_className, "CreateDistributionList", ex);

                return null;
            }

            var newList = new DistributionList
            {
                UserId = member.Id,
                ListState = listState,
                BlobFinal = fileType == Enums.DistributionListFileType.Final ? listNameAsFileName : null,
                BlobWorking = fileType == Enums.DistributionListFileType.Working ? listNameAsFileName : null,
                BlobErrors = fileType == Enums.DistributionListFileType.Errors ? listNameAsFileName : null,
                Name = listName
            };

            return SaveDistributionList(newList);
        }

        public IDistributionList UpdateDistributionList(IDistributionList distributionList, byte[] bytes, string contentType, Enums.DistributionListFileType fileType)
        {
            string listNameAsFileName = convertToFileName(distributionList.Name, contentType, fileType);
            var uploadedListName = $"{distributionList.UserId}/{listNameAsFileName}";

            var methodName = "UpdateDistributionList";
            if (!string.IsNullOrEmpty(distributionList.BlobFinal) &&
                distributionList.BlobFinal.EndsWith(listNameAsFileName))
            {
                _logger.Info(_className, methodName,
                             "Attempting to replace existing file {0} on list: {1}:{2}", listNameAsFileName,
                             distributionList.UserId, distributionList.DistributionListId);
            }
            else
            {
                _logger.Info(_className, methodName, "Uploading new file {0} on list: {1}:{2}",
                             listNameAsFileName, distributionList.UserId, distributionList.DistributionListId);
            }

            try
            {
                _blobStorage.StoreBytes(bytes, uploadedListName, contentType);

                string blobToClean = null;

                switch (fileType)
                {
                    case Enums.DistributionListFileType.Errors:
                        if (distributionList.BlobErrors != null && !distributionList.BlobErrors.Equals(listNameAsFileName,
                                                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            blobToClean = distributionList.BlobErrors;
                        }

                        distributionList.BlobErrors = listNameAsFileName;
                        break;
                    case Enums.DistributionListFileType.Working:
                        if (distributionList.BlobWorking != null && !distributionList.BlobWorking.Equals(listNameAsFileName,
                                                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            blobToClean = distributionList.BlobWorking;
                        }

                        distributionList.BlobWorking = listNameAsFileName;
                        break;
                    case Enums.DistributionListFileType.Final:
                        if (distributionList.BlobFinal != null && !distributionList.BlobFinal.Equals(listNameAsFileName,
                                                                StringComparison.InvariantCultureIgnoreCase))
                        {
                            blobToClean = distributionList.BlobFinal;
                        }

                        distributionList.BlobFinal = listNameAsFileName;
                        distributionList.ListState = Enums.DistributionListState.Complete;
                        break;
                }

                if (!string.IsNullOrEmpty(blobToClean))
                {
                    _logger.Info(_className, methodName,
                                 "Cleaning old version of file {0}:{1} for list {2}:{3}", fileType, blobToClean,
                                 distributionList.UserId, distributionList.DistributionListId);

                    _blobStorage.DeleteBlob($"{distributionList.UserId}/{blobToClean}");
                }

                if (fileType == Enums.DistributionListFileType.Final && (!string.IsNullOrEmpty(distributionList.BlobErrors) || !string.IsNullOrEmpty(distributionList.BlobWorking)))
                {
                    // Clean up any remaining Working/Errors files
                    _logger.Info(_className, methodName,
                                 "New final list has been uploaded, attempting to remove any old working files for list: {0}:{1}", distributionList.UserId,
                                 distributionList.DistributionListId);

                    if (!string.IsNullOrEmpty(distributionList.BlobErrors))
                    {
                        _blobStorage.DeleteBlob($"{distributionList.UserId}/{distributionList.BlobErrors}");

                        _logger.Info(_className, methodName,
                                     "Successfully removed Errors file {0} for list: {1}:{2}",
                                     distributionList.BlobErrors, distributionList.UserId,
                                     distributionList.DistributionListId);

                        distributionList.BlobErrors = null;
                    }

                    if (!string.IsNullOrEmpty(distributionList.BlobWorking))
                    {
                        _blobStorage.DeleteBlob($"{distributionList.UserId}/{distributionList.BlobWorking}");
                        _logger.Info(_className, methodName,
                                     "Successfully removed Working file {0} for list: {1}:{2}",
                                     distributionList.BlobWorking, distributionList.UserId,
                                     distributionList.DistributionListId);

                        distributionList.BlobWorking = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Exception(_className, "CreateDistributionList", ex);

                return null;
            }

            return SaveDistributionList(distributionList);
        }

        public byte[] GetDataFile(IDistributionList distributionList, Enums.DistributionListFileType fileType)
        {
            var uploadedListName = $"{distributionList.UserId}/";

            switch (fileType)
            {
                case Enums.DistributionListFileType.Final:
                    uploadedListName = uploadedListName + distributionList.BlobFinal;
                    break;
                case Enums.DistributionListFileType.Errors:
                    uploadedListName = uploadedListName + distributionList.BlobErrors;
                    break;
                case Enums.DistributionListFileType.Working:
                    uploadedListName = uploadedListName + distributionList.BlobWorking;
                    break;
            }

            return _blobStorage.FetchBytes(uploadedListName);
        }

        public void AbandonContactEdits(IDistributionList distributionList)
        {
            if (!string.IsNullOrEmpty(distributionList.BlobWorking))
            {
                _blobStorage.DeleteBlob($"{distributionList.UserId}/{distributionList.BlobWorking}");
                distributionList.BlobWorking = null;
            }

            if (!string.IsNullOrEmpty(distributionList.BlobErrors))
            {
                _blobStorage.DeleteBlob($"{distributionList.UserId}/{distributionList.BlobErrors}");
                distributionList.BlobErrors = null;
            }

            if (!string.IsNullOrEmpty(distributionList.BlobFinal))
            {
                // Reset back to complete
                distributionList.ListState = Enums.DistributionListState.Complete;
                SaveDistributionList(distributionList);
            }
            else
            {
                // Throw it away
                DeleteDistributionList(distributionList);
            }
        }

        public IDistributionList CreateWorkingXml<T>(IDistributionList distributionList, int contactsCount,
                                                  IEnumerable<T> contacts) where T : IDistributionContact
        {
            if (contactsCount == 0)
            {
                return distributionList;
            }

            IDistributionList updatedList = distributionList;

            updatedList.ListState = Enums.DistributionListState.FixIssues;

            // Convert Successful items into an XML doc
            var successfulXml = new XDocument();
            var distributionListElement = new XElement(_elementDistributionList,
                                                       new XAttribute(_attributeListName, updatedList.Name),
                                                       new XAttribute(_attributeCount, contactsCount));

            DataContractSerializer serialiser = new DataContractSerializer(typeof(T));

            using (var successWriter = distributionListElement.CreateWriter())
            {
                foreach (var contact in contacts)
                {
                    serialiser.WriteObject(successWriter, contact);
                }
            }

            successfulXml.Add(distributionListElement);

            using (var successfulStream = new MemoryStream())
            {
                successfulXml.Save(successfulStream);
                successfulStream.Position = 0;

                updatedList = UpdateDistributionList(updatedList, successfulStream.ToArray(),
                                                     PCL.Constants.MimeTypes.Xml,
                                                     Enums.DistributionListFileType.Working);
            }

            return updatedList;
        }

        public IDistributionList CreateErrorXml<T>(IDistributionList distributionList, int errorsCount, IEnumerable<T> errorContacts,
                                                int duplicatesCount, IEnumerable<T> duplicateContacts) where T : IDistributionContact
        {
            if (errorsCount == 0 && duplicatesCount == 0)
            {
                return distributionList;
            }

            IDistributionList updatedList = distributionList;

            updatedList.ListState = Enums.DistributionListState.FixIssues;

            var errorsXml = new XDocument();
            var errorElement = new XElement(_elementErrors);
            errorsXml.Add(errorElement);

            var serialiser = new DataContractSerializer(typeof(T));

            if (errorsCount > 0)
            {
                var invalidElement = new XElement(_elementInvalid, new XAttribute(_attributeListName, updatedList.Name),
                                                 new XAttribute(_attributeCount, errorsCount));

                using (var errorWriter = invalidElement.CreateWriter())
                {
                    foreach (var contact in errorContacts)
                    {
                        serialiser.WriteObject(errorWriter, contact);
                    }
                }

                errorElement.Add(invalidElement);
            }

            if (duplicatesCount > 0)
            {
                var duplicateElement = new XElement(_elementDuplicates, new XAttribute(_attributeListName, updatedList.Name),
                                                 new XAttribute(_attributeCount, duplicatesCount));

                using (var dupWriter = duplicateElement.CreateWriter())
                {
                    foreach (var contact in duplicateContacts)
                    {
                        serialiser.WriteObject(dupWriter, contact);
                    }
                }

                errorElement.Add(duplicateElement);
            }

            using (var errorsStream = new MemoryStream())
            {
                errorsXml.Save(errorsStream);
                errorsStream.Position = 0;

                updatedList = UpdateDistributionList(updatedList, errorsStream.ToArray(), PCL.Constants.MimeTypes.Xml,
                                                    Enums.DistributionListFileType.Errors);
            }

            return updatedList;
        }

        public IModifyListSummaryModel<T> CreateSummaryModel<T>(IDistributionList distributionList) where T : IDistributionContact
        {
            List<T> nullList;
            return getAllDetailsFromWorkingFiles(distributionList, false, out nullList);
        }

        private IModifyListSummaryModel<T> getAllDetailsFromWorkingFiles<T>(IDistributionList distributionList, bool includeValidList, out List<T> validContacts)
            where T : IDistributionContact
        {
            validContacts = includeValidList ? new List<T>() : null;

            var summaryModel = new ModifyListSummaryModel<T>();

            // Grab the files
            if (!string.IsNullOrEmpty(distributionList.BlobWorking))
            {
                byte[] validData = GetDataFile(distributionList,
                                               Enums.DistributionListFileType.Working);

                using (var validStream = new MemoryStream(validData))
                {
                    using (var validReader = new StreamReader(validStream))
                    {
                        var validXml = XDocument.Load(validReader);

                        var distributionListElement = validXml.Element(_elementDistributionList);

                        if (distributionListElement == null)
                        {
                            _logger.Critical(GetType().Name, "ShowSummaryListForm",
                                             "Unable to load working XML document for user list: {0}:{1} - {2} ",
                                             distributionList.UserId, distributionList.DistributionListId,
                                             distributionList.BlobWorking);
                            throw new ArgumentException();
                        }

                        if (includeValidList)
                        {
                            var serialiser = new DataContractSerializer(typeof(T));

                            validContacts = new List<T>(summaryModel.ValidContactCount);

                            foreach (var validContact in distributionListElement.Elements())
                            {
                                using (var validXmlReader = validContact.CreateReader())
                                {
                                    validContacts.Add((T)serialiser.ReadObject(validXmlReader));
                                }
                            }
                        }

                        summaryModel.ValidContactCount = (int)distributionListElement.Attribute("count");
                    }
                }
            }

            if (!string.IsNullOrEmpty(distributionList.BlobErrors))
            {
                byte[] errorData = GetDataFile(distributionList, Enums.DistributionListFileType.Errors);

                var serialiser = new DataContractSerializer(typeof (T));

                using (var errorStream = new MemoryStream(errorData))
                {
                    using (var errorReader = new StreamReader(errorStream))
                    {
                        var errorXml = XDocument.Load(errorReader);

                        var errorElement = errorXml.Element(_elementErrors);

                        if (errorElement == null)
                        {
                            _logger.Critical(GetType().Name, "ShowSummaryListForm",
                                             "Unable to load error XML document for user list: {0}:{1} - {2} ",
                                             distributionList.UserId, distributionList.DistributionListId,
                                             distributionList.BlobErrors);
                            throw new ArgumentException();
                        }

                        var invalidElement = errorElement.Element(_elementInvalid);

                        if (invalidElement != null && invalidElement.Descendants().Any())
                        {
                            summaryModel.InvalidContactCount =
                                (int)invalidElement.Attribute(_attributeCount);

                            var invalidContacts = new List<T>(summaryModel.InvalidContactCount);

                            foreach (var invalidContact in invalidElement.Elements())
                            {
                                using (var invalidXeReader = invalidContact.CreateReader())
                                {
                                    invalidContacts.Add((T)serialiser.ReadObject(invalidXeReader));
                                }
                            }

                            summaryModel.InvalidContacts = invalidContacts;
                        }

                        var duplicateElement = errorElement.Element(_elementDuplicates);

                        if (duplicateElement != null && duplicateElement.Descendants().Any())
                        {
                            summaryModel.DuplicateContactCount =
                                (int)duplicateElement.Attribute(_attributeCount);

                            var duplicateContacts = new List<T>(summaryModel.DuplicateContactCount);

                            foreach (var duplicateContact in duplicateElement.Elements())
                            {
                                using (var duplicateXeReader = duplicateContact.CreateReader())
                                {
                                    duplicateContacts.Add((T)serialiser.ReadObject(duplicateXeReader));
                                }
                            }

                            summaryModel.DuplicateContacts = duplicateContacts;
                        }
                    }
                }
            }

            summaryModel.TotalContactCount = distributionList.RecordCount + summaryModel.ValidContactCount;

            return summaryModel;
        }

        public IDistributionList CompleteContactEdits(IDistributionList distributionList)
        {
            byte[] data = GetDataFile(distributionList, Enums.DistributionListFileType.Working);

            using (var validStream = new MemoryStream(data))
            {
                using (var validReader = new StreamReader(validStream))
                {
                    var validXml = XDocument.Load(validReader);

                    var distributionListElement = validXml.Element(_elementDistributionList);

                    if (distributionListElement == null)
                    {
                        _logger.Critical(_className, "CompleteContactEdits",
                                         "Unable to load working XML document for user list: {0}:{1} - {2} ",
                                         distributionList.UserId, distributionList.DistributionListId,
                                         distributionList.BlobWorking);
                        throw new ArgumentException();
                    }

                    distributionList.RecordCount = distributionList.RecordCount + (int)distributionListElement.Attribute("count");
                }
            }

            return UpdateDistributionList(distributionList, data, PCL.Constants.MimeTypes.Xml,
                                          Enums.DistributionListFileType.Final);
        }

        public IModifyListSummaryModel<T> UpdateWorkingXml<T>(IDistributionList distributionList, List<T> contactsUpdate) where T : IDistributionContact
        {
            // Grab the existing summary model, will load core data and grab and parse the errors file for us
            List<T> validContacts;
            IModifyListSummaryModel<T> summaryModel = getAllDetailsFromWorkingFiles(distributionList, true, out validContacts);

            // See if the contact exists in the errors list
            List<T> invalidContacts = summaryModel.InvalidContacts?.ToList() ?? new List<T>();
            List<T> duplicateContacts = summaryModel.DuplicateContacts?.ToList() ?? new List<T>();

            int removedErrors = 0;
            int addedDuplicates = 0;

            // Do we have any contacts with a valid contact id, and do we have any invalid contacts that these could be replacing?
            if (contactsUpdate.Any(c => c.ContactId != Guid.Empty) && invalidContacts.Any())
            {
                var errorsToRemove = invalidContacts.Intersect(contactsUpdate).ToList();

                if (errorsToRemove.Any())
                {
                    foreach (var error in errorsToRemove)
                    {
                        invalidContacts.Remove(error);
                        removedErrors--;
                    }
                }
            }

            int successfulAdd = 0;
            int invalidAdd = 0;
            
            foreach (var contact in contactsUpdate)
            {
                ICollection<ValidationResult> results;
                bool isValid = contact.TryValidate(out results);

                if (!isValid)
                {
                    invalidContacts.Add(contact);
                    invalidAdd++;
                    continue;
                }

                if (!validContacts.Any() || validContacts.All(vc => vc.AddressRef != contact.AddressRef))
                {
                    validContacts.Add(contact);
                    successfulAdd++;
                }
                else
                {
                    duplicateContacts.Add(contact);
                    addedDuplicates++;
                }
            }

            if (successfulAdd > 0)
            {
                distributionList = CreateWorkingXml(distributionList,  summaryModel.ValidContactCount + successfulAdd, validContacts);
            }

            if (removedErrors < 0 || invalidAdd > 0 || addedDuplicates > 0)
            {
                distributionList = CreateErrorXml(distributionList, summaryModel.InvalidContactCount + removedErrors + invalidAdd,
                                                  invalidContacts, summaryModel.DuplicateContactCount + addedDuplicates,
                                                  duplicateContacts);
            }

            summaryModel.DuplicateContactCount = summaryModel.DuplicateContactCount + addedDuplicates;
            summaryModel.DuplicateContactsAdded = addedDuplicates;
            summaryModel.DuplicateContacts = duplicateContacts;
            summaryModel.InvalidContactCount = summaryModel.InvalidContactCount + removedErrors + invalidAdd;
            summaryModel.InvalidContactsAdded = removedErrors + invalidAdd;
            summaryModel.InvalidContacts = invalidContacts;
            summaryModel.ValidContactCount = summaryModel.ValidContactCount + successfulAdd;
            summaryModel.ValidContactsAdded = successfulAdd;
            summaryModel.TotalContactCount = distributionList.RecordCount + summaryModel.ValidContactCount;

            return summaryModel;
        }

        private static string convertToFileName(string listName, string contentType, Enums.DistributionListFileType fileType)
        {
            string fileName = Path.GetInvalidFileNameChars().Aggregate(listName, (current, c) => current.Replace(c, '_')).Replace(" ", "_");
            string extension = contentType.Equals(PCL.Constants.MimeTypes.Xml) ? "xml" : "csv";

            return $"{fileName}-{fileType}.{extension}";
        }
    }
}
