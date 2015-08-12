using HC.RM.Common.Azure.Persistence;
using HC.RM.Common.PCL.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Helpers
{
    public class BlobStorageHelper
    {
        private IBlobStorage blobStore;
        private IBlobService blobService;
        private string _containerName;

        public BlobStorageHelper(string connectionString, string containerName)
        {
            _containerName = containerName;
            blobStore = new BlobStorage(connectionString);
            blobService = new BlobService(blobStore, containerName);
        }

        public void ChangeContainer(string containerName)
        {
            _containerName = containerName;
            blobService = new BlobService(blobStore, containerName);
        }

        public byte[] FetchBytes(string blobId)
        {
            var blobStream = blobService.DownloadToStream(blobId);
            if (blobStream is MemoryStream)
            {
                return ((MemoryStream)blobStream).ToArray();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                blobStream.CopyTo(ms);
                var result = ms.ToArray();
                return result;
            }
        }

        public string StoreBytes(byte[] bytes, string filename, string mediaType)
        {
            var blobId = blobService.Store(bytes, filename, mediaType);
            var url = blobService.GetBlobUri(_containerName, filename);
            return url.ToString();
        }

    }
}
