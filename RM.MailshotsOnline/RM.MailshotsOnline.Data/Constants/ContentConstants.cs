using System.Configuration;

namespace RM.MailshotsOnline.Data.Constants
{
    public static class ContentConstants
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
            public static readonly int PublicMediaLibraryId = int.Parse(ConfigurationManager.AppSettings["PublicMediaLibraryId"]);
            public static readonly int PrivateMediaLibraryId = int.Parse(ConfigurationManager.AppSettings["PrivateMediaLibraryId"]);
            public static readonly string PublicLibraryImageMediaTypeAlias = ConfigurationManager.AppSettings["PublicLibraryImageMediaTypeAlias"];
            public static readonly string PrivateLibraryImageMediaTypeAlias = ConfigurationManager.AppSettings["PrivateLibraryImageMediaTypeAlias"];
        }
    }
}
