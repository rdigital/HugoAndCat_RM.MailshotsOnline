using System.IO;
using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.PCL.Persistence;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System.Collections.Generic;

namespace RM.MailshotsOnline.Data.Helpers
{
    public class BlobStorageHelper
    {
        private readonly IBlobStorage _blobStore;
        private IBlobService _blobService;
        private string _containerName;
        private CloudStorageAccount _cloudStorageAccount;
        private CloudBlobClient _cloudBlobClient;
        private CloudBlobContainer _cloudBlobContainer;

        public BlobStorageHelper(string connectionString, string containerName)
        {
            _containerName = containerName;
            _blobStore = new BlobStorage(connectionString);
            _blobService = new BlobService(_blobStore, containerName);

            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
        }

        public void ChangeContainer(string containerName)
        {
            _containerName = containerName;
            _blobService = new BlobService(_blobStore, containerName);
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerName);
        }

        public void SetCorsAccess(string containerName)
        {
            ServiceProperties blobServiceProperties = _cloudBlobClient.GetServiceProperties();
            blobServiceProperties.Cors = new CorsProperties();
            blobServiceProperties.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedHeaders = new List<string>() { "*" },
                AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Head,
                AllowedOrigins = new List<string>() { "*" },
                ExposedHeaders = new List<string>() { "*" },
                MaxAgeInSeconds = 1800
            });

            _cloudBlobClient.SetServiceProperties(blobServiceProperties, new BlobRequestOptions() {  }, new OperationContext() {  } );
        }

        public byte[] FetchBytes(string blobId)
        {
            var blobStream = _blobService.DownloadToStream(blobId);
            if (blobStream != null)
            {
                if (blobStream is MemoryStream)
                {
                    return ((MemoryStream)blobStream).ToArray();
                }

                using (var ms = new MemoryStream())
                {
                    blobStream.CopyTo(ms);
                    var result = ms.ToArray();
                    return result;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<byte[]> FetchBytesAsync(string blobId)
        {
            var blobStream = await _blobService.DownloadToStreamAsync(blobId);
            if (blobStream is MemoryStream)
            {
                return ((MemoryStream)blobStream).ToArray();
            }

            using (var ms = new MemoryStream())
            {
                blobStream.CopyTo(ms);
                var result = ms.ToArray();
                return result;
            }
        }

        public string GetBlobUrl(string blobId)
        {
            var blobUri = _blobService.GetBlobUri(_containerName, blobId);
            if (blobUri != null)
            {
                return blobUri.ToString();
            }

            return null;
        }

        public string StoreBytes(byte[] bytes, string filename, string mediaType)
        {
            var blobId = _blobService.Store(bytes, filename, mediaType);
            var url = _blobService.GetBlobUri(_containerName, blobId);
            return url.ToString();
        }

        public async Task<string> StoreBytesAsync(byte[] bytes, string filename, string mediaType)
        {
            var blobId = await _blobService.StoreAsync(bytes, filename, mediaType);
            var url = _blobService.GetBlobUri(_containerName, blobId);
            return url.ToString();
        }

        public string GetBlobUrlWithSas(string blobId, int secondsValid)
        {
            return _blobService.GetBlobUriWithReadSas(blobId, new TimeSpan(0, 0, secondsValid)).ToString();
            //var url = new StorageUri(new System.Uri(GetBlobUrl(blobId)));
            //var blob = _cloudBlobClient.GetBlobReferenceFromServer(url);
            //var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            //{
            //    Permissions = SharedAccessBlobPermissions.Read,
            //    SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
            //    SharedAccessExpiryTime = DateTime.UtcNow.AddSeconds(secondsValid)
            //});
            //return string.Format("{0}{1}", blob.Uri, sasToken);
        }

        public void DeleteBlob(string blobId)
        {
            if (!string.IsNullOrEmpty(blobId))
            {
                _blobService.DeleteBlob(blobId);
                //var url = new StorageUri(new System.Uri(GetBlobUrl(blobId)));
                //if (url != null)
                //{
                //    var blob = _cloudBlobClient.GetBlobReferenceFromServer(url);
                //    if (blob != null)
                //    {
                //        blob.Delete();
                //    }
                //}
            }
        }
    }
}
