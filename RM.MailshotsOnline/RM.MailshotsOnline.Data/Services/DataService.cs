using System;
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

        private readonly string _className = "DataService";
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
            return GetDistributionLists(d => d.UserId == userId).OrderByDescending(d=> d.UpdatedDate);
        }

        public bool ListNameIsAlreadyInUse(int userId, string listName)
        {
            return GetDistributionListsForUser(userId).Any(d => string.Equals(d.Name, listName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<IDistributionList> GetDistributionLists(Func<IDistributionList, bool> filter)
        {
            return _context.DistributionLists.Where(filter);
        }

        public IDistributionList SaveDistributionList(IDistributionList distributionList, bool updateTimestamp = true)
        {
            if (updateTimestamp)
            {
                distributionList.UpdatedDate = DateTime.UtcNow;
            }

            if (distributionList.DistributionListId == Guid.Empty)
            {
                _context.DistributionLists.Add((DistributionList)distributionList);
            }

            _context.SaveChanges();

            return distributionList;
        }

        public void DeleteDistributionList(IDistributionList distributionList)
        {
            var methodName = "DeleteDistributionList";

            _logger.Info(_className, methodName, "Deleting list: {0}", distributionList.DistributionListId);

            deleteBlob(distributionList, Enums.DistributionListFileType.Working);
            deleteBlob(distributionList, Enums.DistributionListFileType.Errors);
            deleteBlob(distributionList, Enums.DistributionListFileType.Final);

            _context.DistributionLists.Remove((DistributionList)distributionList);

            _context.SaveChanges();
        }

        /// <summary>
        /// Checks if a distribution list is assigned to any campaigns
        /// </summary>
        /// <param name="distributionList">The distribution list to check</param>
        /// <returns>True if the list is used, false otherwise</returns>
        public bool DistributionListAssignedToCampaign(IDistributionList distributionList)
        {
            return _context.CampaignDistributionLists.Any(cdl => cdl.DistributionListId == distributionList.DistributionListId);
        }

        private IDistributionList deleteBlob(IDistributionList distributionList, Enums.DistributionListFileType fileType)
        {
            var updatedList = distributionList;

            switch (fileType)
            {
                case Enums.DistributionListFileType.Errors:
                    deleteBlob(updatedList.BlobErrors, updatedList.UserId, fileType);
                    updatedList.BlobErrors = null;
                    break;
                case Enums.DistributionListFileType.Working:
                    deleteBlob(updatedList.BlobWorking, updatedList.UserId, fileType);
                    updatedList.BlobWorking = null;
                    break;
                case Enums.DistributionListFileType.Final:
                    deleteBlob(updatedList.BlobFinal, updatedList.UserId, fileType);
                    updatedList.BlobFinal= null;
                    break;
            }

            return updatedList;
        }

        private void deleteBlob(string blobReference, int userId, Enums.DistributionListFileType fileType)
        {
            if (!string.IsNullOrEmpty(blobReference))
            {
                _logger.Info(_className, "deleteBlob",
                             "Deleting file: {0}:{1}\\{1}", fileType,
                             userId, blobReference);

                _blobStorage.DeleteBlob($"{userId}/{blobReference}");
            }
        }

        public IDistributionList CreateDistributionList(IMember member, string listName, Enums.DistributionListState listState, byte[] bytes, string contentType, Enums.DistributionListFileType fileType)
        {
            string listNameAsFileName = convertToFileName(listName, contentType, fileType);
            var uploadedListName = $"{member.Id}/{listNameAsFileName}";

            _logger.Info(_className, "CreateDistributionList", "Creating new list for user: {0}, listName: {1}", member.Id, listName);

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

            bool updateTimestamp = false;

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
                        updateTimestamp = true;
                        break;
                }

                if (!string.IsNullOrEmpty(blobToClean))
                {
                    _logger.Info(_className, methodName,
                                 "Cleaning old version of file {0}:{1} for list {2}:{3}", fileType, blobToClean,
                                 distributionList.UserId, distributionList.DistributionListId);

                    deleteBlob(blobToClean, distributionList.UserId, fileType);
                }

                if (fileType == Enums.DistributionListFileType.Final && (!string.IsNullOrEmpty(distributionList.BlobErrors) || !string.IsNullOrEmpty(distributionList.BlobWorking)))
                {
                    // Clean up any remaining Working/Errors files
                    _logger.Info(_className, methodName,
                                 "New final list has been uploaded, attempting to remove any old working files for list: {0}:{1}", distributionList.UserId,
                                 distributionList.DistributionListId);

                    distributionList = deleteBlob(distributionList, Enums.DistributionListFileType.Errors);

                    distributionList = deleteBlob(distributionList, Enums.DistributionListFileType.Working);
                }
            }
            catch (Exception ex)
            {
                _logger.Exception(_className, methodName, ex);

                return null;
            }

            return SaveDistributionList(distributionList, updateTimestamp);
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
            _logger.Info(_className, "AbandonContactEdits", "Abandoning contact edits on user list: {0}:{1}",
                         distributionList.UserId, distributionList.DistributionListId);

            if (!string.IsNullOrEmpty(distributionList.BlobFinal))
            {
                // Reset back to complete
                distributionList = deleteBlob(distributionList, Enums.DistributionListFileType.Working);
                distributionList = deleteBlob(distributionList, Enums.DistributionListFileType.Errors);

                distributionList.ListState = Enums.DistributionListState.Complete;
                SaveDistributionList(distributionList, false);
            }
            else
            {
                // Throw it away
                DeleteDistributionList(distributionList);
            }
        }

        public IDistributionList CreateValidXml<T>(IDistributionList distributionList, int contactsCount, IEnumerable<T> contacts, Enums.DistributionListFileType fileType = Enums.DistributionListFileType.Working) where T : IDistributionContact
        {
            if (contactsCount == 0)
            {
                return distributionList;
            }

            IDistributionList updatedList = distributionList;

            updatedList.ListState = Enums.DistributionListState.FixIssues;

            // Convert Successful items into an XML doc
            var successfulXml = new XDocument();
            var distributionListElement = createDistributionListElement(contactsCount, contacts, updatedList, _elementDistributionList);

            successfulXml.Add(distributionListElement);

            using (var successfulStream = new MemoryStream())
            {
                successfulXml.Save(successfulStream);
                successfulStream.Position = 0;

                updatedList = UpdateDistributionList(updatedList, successfulStream.ToArray(),
                                                     PCL.Constants.MimeTypes.Xml,
                                                     fileType);
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
                var invalidElement = createDistributionListElement(errorsCount, errorContacts, updatedList,
                                                                   _elementInvalid, serialiser);
                
                errorElement.Add(invalidElement);
            }

            if (duplicatesCount > 0)
            {
                var duplicateElement = createDistributionListElement(duplicatesCount, duplicateContacts, updatedList,
                                                                     _elementDuplicates, serialiser);

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

        public IDistributionList CompleteContactEdits(IDistributionList distributionList)
        {
            if (string.IsNullOrEmpty(distributionList.BlobWorking))
            {
                AbandonContactEdits(distributionList);

                return null;
            }

            // TODO: If there's no existing final list to merge into, just mark Working as Final?
            // DO NOT TRY AND COPY/RENAME THE BLOB: http://stackoverflow.com/a/26549519/33051
            byte[] data = GetDataFile(distributionList, Enums.DistributionListFileType.Working);

            XElement workingListElement;
            using (var validStream = new MemoryStream(data))
            {
                using (var validReader = new StreamReader(validStream))
                {
                    var validXml = XDocument.Load(validReader);

                    workingListElement = validXml.Element(_elementDistributionList);

                    if (workingListElement == null)
                    {
                        _logger.Critical(_className, "CompleteContactEdits",
                                         "Unable to load working XML document for user list: {0}:{1} - {2} ",
                                         distributionList.UserId, distributionList.DistributionListId,
                                         distributionList.BlobWorking);
                        throw new ArgumentException();
                    }

                    distributionList.RecordCount = distributionList.RecordCount + (int)workingListElement.Attribute("count");
                }
            }

            XDocument finalXml = null;
            if (!string.IsNullOrEmpty(distributionList.BlobFinal))
            {
                data = GetDataFile(distributionList, Enums.DistributionListFileType.Final);
                
                using (var finalStream = new MemoryStream(data))
                {
                    using (var finalReader = new StreamReader(finalStream))
                    {
                        finalXml = XDocument.Load(finalReader);

                        var existingListElement = finalXml.Element(_elementDistributionList);

                        if (existingListElement == null)
                        {
                            _logger.Critical(_className, "CompleteContactEdits",
                                             "Unable to load existing final XML document for user list: {0}:{1} - {2} ",
                                             distributionList.UserId, distributionList.DistributionListId,
                                             distributionList.BlobFinal);
                            throw new ArgumentException();
                        }

                        existingListElement.Add(workingListElement.Elements());
                    }
                }
            }

            if (finalXml == null)
            {
                return UpdateDistributionList(distributionList, data, PCL.Constants.MimeTypes.Xml,
                                              Enums.DistributionListFileType.Final);
            }

            using (var saveStream = new MemoryStream())
            {
                finalXml.Save(saveStream);
                saveStream.Position = 0;

                return UpdateDistributionList(distributionList, saveStream.ToArray(),
                                              PCL.Constants.MimeTypes.Xml,
                                              Enums.DistributionListFileType.Working);
            }
        }

        public IModifyListSummaryModel<T> UpdateWorkingXml<T>(IDistributionList distributionList, IModifyListMappedFieldsModel<T> contactsUpdate) where T : IDistributionContact
        {
            // Grab the existing summary model, will load core data and grab and parse the errors file for us
            List<T> validContacts;
            IModifyListSummaryModel<T> summaryModel = getAllDetailsFromWorkingFiles(distributionList, true, out validContacts);

            // See if the contact exists in the errors list
            var invalidContacts = summaryModel.InvalidContacts?.ToList() ?? new List<T>();
            var duplicateContacts = summaryModel.DuplicateContacts?.ToList() ?? new List<T>();

            int removedErrors = 0;

            // Do we have any contacts with a valid contact id, and do we have any invalid contacts that these could be replacing?
            if (contactsUpdate.ValidContacts.Any(c => c.ContactId != Guid.Empty) && invalidContacts.Any())
            {
                var errorsToRemove = invalidContacts.Intersect(contactsUpdate.ValidContacts).ToList();

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
            int invalidAdd = contactsUpdate.InvalidContactsCount;
            int addedDuplicates = contactsUpdate.DuplicateContactsCount;

            foreach (var contact in contactsUpdate.ValidContacts)
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

            invalidContacts.AddRange(contactsUpdate.InvalidContacts);
            
            duplicateContacts.AddRange(contactsUpdate.DuplicateContacts);

            if (successfulAdd > 0)
            {
                distributionList = CreateValidXml(distributionList, summaryModel.ValidContactCount + successfulAdd, validContacts);
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

        public List<T> GetFinalContacts<T>(IDistributionList distributionList) where T : IDistributionContact
        {
            if (!string.IsNullOrEmpty(distributionList.BlobFinal))
            {
                byte[] finalData = GetDataFile(distributionList, Enums.DistributionListFileType.Final);

                var countAndContacts = getCountAndContacts<T>(true, finalData);

                return countAndContacts.Contacts?.ToList() ?? new List<T>();
            }

            return new List<T>();
        }

        public IDistributionList UpdateFinalXml<T>(IDistributionList distributionList, int contactsCount, IEnumerable<T> contacts) where T : IDistributionContact
        {
            XDocument finalXml;

            var finalElement = createDistributionListElement(contactsCount, contacts, distributionList,
                                                             _elementDistributionList);

            if (!string.IsNullOrEmpty(distributionList.BlobFinal))
            {
                // Merge into existing final list
                byte[] data = GetDataFile(distributionList, Enums.DistributionListFileType.Final);

                using (var finalStream = new MemoryStream(data))
                {
                    using (var finalReader = new StreamReader(finalStream))
                    {
                        finalXml = XDocument.Load(finalReader);

                        var existingListElement = finalXml.Element(_elementDistributionList);

                        if (existingListElement == null)
                        {
                            _logger.Critical(_className, "UpdateFinalXml",
                                             "Unable to load existing final XML document for user list: {0}:{1} - {2} ",
                                             distributionList.UserId, distributionList.DistributionListId,
                                             distributionList.BlobFinal);
                            throw new ArgumentException();
                        }

                        existingListElement.Add(finalElement.Elements());
                    }
                }
            }
            else
            {
                finalXml = new XDocument();
                finalXml.Add(finalElement);
            }

            using (var saveStream = new MemoryStream())
            {
                finalXml.Save(saveStream);
                saveStream.Position = 0;

                return UpdateDistributionList(distributionList, saveStream.ToArray(),
                                              PCL.Constants.MimeTypes.Xml,
                                              Enums.DistributionListFileType.Final);
            }
        }

        private IModifyListSummaryModel<T> getAllDetailsFromWorkingFiles<T>(IDistributionList distributionList, bool includeValidList, out List<T> validContacts) where T : IDistributionContact
        {
            validContacts = includeValidList ? new List<T>() : null;

            var summaryModel = new ModifyListSummaryModel<T>
            {
                DistributionListId = distributionList.DistributionListId,
                ListName = distributionList.Name,
            };

            // Grab the files
            if (!string.IsNullOrEmpty(distributionList.BlobWorking))
            {
                byte[] validData = GetDataFile(distributionList,
                                               Enums.DistributionListFileType.Working);

                var countAndContacts = getCountAndContacts<T>(includeValidList, validData);

                summaryModel.ValidContactCount = countAndContacts.Count;
                validContacts = countAndContacts.Contacts?.ToList();
            }

            if (!string.IsNullOrEmpty(distributionList.BlobErrors))
            {
                byte[] errorData = GetDataFile(distributionList, Enums.DistributionListFileType.Errors);

                using (var errorStream = new MemoryStream(errorData))
                {
                    using (var errorReader = new StreamReader(errorStream))
                    {
                        var errorXml = XDocument.Load(errorReader);

                        var errorElement = errorXml.Element(_elementErrors);

                        if (errorElement == null)
                        {
                            _logger.Critical(GetType().Name, "getAllDetailsFromWorkingFiles",
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

                            summaryModel.InvalidContacts = getContactsFromXElement<T>(invalidElement,
                                                                                      summaryModel.InvalidContactCount);
                        }

                        var duplicateElement = errorElement.Element(_elementDuplicates);

                        if (duplicateElement != null && duplicateElement.Descendants().Any())
                        {
                            summaryModel.DuplicateContactCount =
                                (int)duplicateElement.Attribute(_attributeCount);

                            summaryModel.DuplicateContacts = getContactsFromXElement<T>(duplicateElement,
                                                                                        summaryModel.DuplicateContactCount);
                        }
                    }
                }
            }

            summaryModel.TotalContactCount = distributionList.RecordCount + summaryModel.ValidContactCount;

            return summaryModel;
        }

        private CountAndContacts<T> getCountAndContacts<T>(bool includeContacts, byte[] validData)
            where T : IDistributionContact
        {
            List<T> contacts = null;
            int contactCount;
            using (var validStream = new MemoryStream(validData))
            {
                using (var validReader = new StreamReader(validStream))
                {
                    var validXml = XDocument.Load(validReader);

                    var distributionListElement = validXml.Element(_elementDistributionList);

                    if (distributionListElement == null)
                    {
                        _logger.Critical(GetType().Name, "getCountAndContacts", "Unable to load XML document to get contacts");
                        throw new ArgumentException();
                    }

                    contactCount = (int)distributionListElement.Attribute("count");

                    if (includeContacts)
                    {
                        contacts = getContactsFromXElement<T>(distributionListElement, contactCount);
                    }
                }
            }

            return new CountAndContacts<T>(contactCount, contacts);
        }

        private XElement createDistributionListElement<T>(int contactsCount, IEnumerable<T> contacts,
                                                          IDistributionList updatedList, string elementName,
                                                          DataContractSerializer serialiser = null)
            where T : IDistributionContact
        {
            var distributionListElement = new XElement(elementName,
                                                       new XAttribute(_attributeListName, updatedList.Name),
                                                       new XAttribute(_attributeCount, contactsCount));

            if (serialiser == null)
            {
                serialiser = new DataContractSerializer(typeof (T));
            }

            using (var successWriter = distributionListElement.CreateWriter())
            {
                foreach (var contact in contacts)
                {
                    serialiser.WriteObject(successWriter, contact);
                }
            }
            return distributionListElement;
        }

        private static List<T> getContactsFromXElement<T>(XContainer listElement, int contactCount) where T : IDistributionContact
        {
            var serialiser = new DataContractSerializer(typeof (T));

            var contacts = new List<T>(contactCount);

            foreach (var contact in listElement.Elements())
            {
                using (var xmlReader = contact.CreateReader())
                {
                    contacts.Add((T)serialiser.ReadObject(xmlReader));
                }
            }

            return contacts;
        }

        private static string convertToFileName(string listName, string contentType, Enums.DistributionListFileType fileType)
        {
            string fileName = Path.GetInvalidFileNameChars().Aggregate(listName, (current, c) => current.Replace(c, '_')).Replace(" ", "_");
            string extension = contentType.Equals(PCL.Constants.MimeTypes.Xml) ? "xml" : "csv";

            return $"{fileName}-{fileType}.{extension}";
        }
    }

    internal struct CountAndContacts<T> where T: IDistributionContact
    {
        private readonly List<T> _contacts;

        internal int Count { get; }

        internal IEnumerable<T> Contacts => _contacts;

        public CountAndContacts(int count, List<T> contacts)
        {
            Count = count;
            _contacts = contacts;
        }
    }
}
