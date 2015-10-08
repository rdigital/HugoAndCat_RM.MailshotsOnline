using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Test.Mocks
{
    public class MockBlobService : HC.RM.Common.PCL.Persistence.IBlobService
    {
        public void DeleteBlob(string path)
        {
            throw new NotImplementedException();
        }

        public Stream DownloadToStream(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadToStreamAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Uri GetBlobUri(string container, string path)
        {
            return new Uri("http://example/path");
        }

        public Uri GetBlobUriWithReadSas(string path, TimeSpan validityPeriod)
        {
            return new Uri("http://example/path");
        }

        public Uri GetBlobUriWithSas(string path, TimeSpan sasValidityPeriod, bool readAccess, bool writeAccess, bool deleteAccess)
        {
            return new Uri("http://example/path");
        }

        public Uri GetBlobUriWithSas(string name, TimeSpan sasValidityPeriod, bool readAccess, bool writeAccess, bool deleteAccess, string sharedAccessPolicyName = null, string ipAddress = null)
        {
            return new Uri("http://example/path");
        }

        public Uri GetBlobUriWithSas(string name, TimeSpan sasValidityPeriod, bool readAccess, bool writeAccess, bool deleteAccess, string sharedAccessPolicyName, string ipAddressStart, string ipAddressEnd)
        {
            return new Uri("http://example/path");
        }

        public string Store(byte[] bytes, string fileName, string mediaType)
        {
            return "blobID";
        }

        public Task<string> StoreAsync(byte[] bytes, string fileName, string mediaType)
        {
            throw new NotImplementedException();
        }
    }
}
