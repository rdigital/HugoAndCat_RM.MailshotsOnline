using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.PageModels.Settings;
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

        private ILogger _logger;
        private readonly StorageContext _context;
        private string _className = "DataService";

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

        public void AbondonContactEdits(IDistributionList distributionList)
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

        private static string convertToFileName(string listName, string contentType, Enums.DistributionListFileType fileType)
        {
            string fileName = Path.GetInvalidFileNameChars().Aggregate(listName, (current, c) => current.Replace(c, '_')).Replace(" ", "_");
            string extension = contentType.Equals("text/xml") ? "xml" : "csv";

            return $"{fileName}-{fileType}.{extension}";
        }
    }
}
