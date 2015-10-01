using System.Configuration;

namespace RM.MailshotsOnline.Data.Constants
{
    public static class Constants
    {
        public static class Settings
        {
            public static readonly int HeaderNavSettingsId = int.Parse(ConfigurationManager.AppSettings["HeaderNavSettingsId"]);
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
            public static readonly string PrivateKey = Helpers.ConfigHelper.GetConfigValue("ReportingSftpPrivateKeyLocation");
        }

        public static class Apis
        {
            public static readonly string ReportsApi = Helpers.ConfigHelper.GetConfigValue("ReportsApiUrl");
        }
    }
}
