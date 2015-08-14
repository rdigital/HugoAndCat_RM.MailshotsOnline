using System.IO;
using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.PCL.Persistence;

namespace RM.MailshotsOnline.Data.Helpers
{
    public class BlobStorageHelper
    {
        private readonly IBlobStorage _blobStore;
        private IBlobService _blobService;
        private string _containerName;

        public BlobStorageHelper(string connectionString, string containerName)
        {
            _containerName = containerName;
            _blobStore = new BlobStorage(connectionString);
            _blobService = new BlobService(_blobStore, containerName);
        }

        public void ChangeContainer(string containerName)
        {
            _containerName = containerName;
            _blobService = new BlobService(_blobStore, containerName);
        }

        public byte[] FetchBytes(string blobId)
        {
            var blobStream = _blobService.DownloadToStream(blobId);
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
            return _blobService.GetBlobUri(_containerName, blobId).ToString();
        }

        public string StoreBytes(byte[] bytes, string filename, string mediaType)
        {
            var blobId = _blobService.Store(bytes, filename, mediaType);
            var url = _blobService.GetBlobUri(_containerName, blobId);
            return url.ToString();
        }

    }
}
