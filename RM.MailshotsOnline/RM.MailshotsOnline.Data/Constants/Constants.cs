using System.Configuration;
using System.IO;
using System.Reflection;
using Lucene.Net.Index;
using Microsoft.Azure;

namespace RM.MailshotsOnline.Data.Constants
{
    public static class Constants
    {
        public static class Settings
        {
            public static readonly int HeaderNavSettingsId = Helpers.ConfigHelper.GetConfigValue("HeaderNavSettingsId", 1121);
            public static readonly string DefaultMediaLibraryTagGroup = ConfigurationManager.AppSettings["DefaultMediaLibraryTagGroup"];
            public static readonly int ImageThumbnailSizeSmall = int.Parse(ConfigurationManager.AppSettings["ImageThumbnailSizeSmall"]);
            public static readonly int ImageThumbnailSizeLarge = int.Parse(ConfigurationManager.AppSettings["ImageThumbnailSizeLarge"]);
        }

        public static class HomeContent
        {
            public static readonly int LoginId = int.Parse(ConfigurationManager.AppSettings["LoginId"]);
        }

        public static class MediaContent
        {
            public static readonly int PublicMediaLibraryId = Helpers.ConfigHelper.GetConfigValue("PublicMediaLibraryId", 1176);
            public static readonly string PublicLibraryImageMediaTypeAlias = Helpers.ConfigHelper.GetConfigValue("PublicLibraryImageMediaTypeAlias");
            public static readonly string PublicImageLibraryFolderMediaTypeAlias = Helpers.ConfigHelper.GetConfigValue("PublicImageLibraryFolderMediaTypeAlias");

            public static readonly int PrivateMediaLibraryId = Helpers.ConfigHelper.GetConfigValue("PrivateMediaLibraryId", 1177);
            public static readonly string PrivateLibraryImageMediaTypeAlias = Helpers.ConfigHelper.GetConfigValue("PrivateLibraryImageMediaTypeAlias");
            public static readonly string PrivateImageLibraryFolderMediaTypeAlias = Helpers.ConfigHelper.GetConfigValue("PrivateImageLibraryFolderMediaTypeAlias");
        }

        public static class Encryption
        {
            public static readonly string EncryptionKey = Helpers.ConfigHelper.GetConfigValue("EncryptionKey");
            public static readonly string EmailSaltPadding = Helpers.ConfigHelper.GetConfigValue("EmailSaltPadding");
        }

        public static class Reporting
        {
            public static readonly string SftpUsername = Helpers.ConfigHelper.GetConfigValue("ReportingSftpUsername");
            public static readonly string SftpHostname = Helpers.ConfigHelper.GetConfigValue("ReportingSftpHostname");
            public static readonly string SftpDirectory = Helpers.ConfigHelper.GetConfigValue("ReportingSftpDirectory");
            public static readonly string PrivateKeyFileName = Helpers.ConfigHelper.GetConfigValue("ReportingSftpPrivateKeyLocation");
            public static readonly string BlobContainer = Helpers.ConfigHelper.GetConfigValue("ReportingBlobContainer");
		}

        public static class Products
        {
            public static readonly string PrintSku = Helpers.ConfigHelper.GetConfigValue("PrintSku", "RMMSOL-PRINT");
            public static readonly string PostSku = Helpers.ConfigHelper.GetConfigValue("PostSku", "RMMSOL-POST");
            public static readonly string YourDataSku = Helpers.ConfigHelper.GetConfigValue("YourDataSku", "RMMSOL-YOUR-DATA");
            public static readonly string OurDataSku = Helpers.ConfigHelper.GetConfigValue("OurDataSku", "RMMSOL-DATA-RENTAL");
            public static readonly string MsolServiceFeeSku = Helpers.ConfigHelper.GetConfigValue("MsolServiceFeeSku", "RMMSOL-SERVICE-FEE");
            public static readonly string DataSearchFeeSku = Helpers.ConfigHelper.GetConfigValue("DataSearchFeeSku", "RMMSOL-DATA-SEARCH-FEE");
        }

        public static class Apis
        {
            public static readonly string ReportsApi = string.Format("{0}://{1}:{2}{3}",
                Helpers.ConfigHelper.HostedScheme, 
                Helpers.ConfigHelper.HostedDomain, 
                Helpers.ConfigHelper.HostedPort,
                Helpers.ConfigHelper.GetConfigValue("ReportsApiUrl"));
        }
    }
}
