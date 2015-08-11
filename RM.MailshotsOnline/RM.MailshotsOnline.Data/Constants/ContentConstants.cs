using System.Configuration;

namespace RM.MailshotsOnline.Data.Constants
{
    public static class ContentConstants
    {
        public static class Settings
        {
            public static readonly int HeaderNavSettingsId = int.Parse(ConfigurationManager.AppSettings["HeaderNavSettingsId"]);
            public static readonly string DefaultMediaLibraryTagGroup = ConfigurationManager.AppSettings["DefaultMediaLibraryTagGroup"];
        }

        public static class HomeContent
        {
            public static readonly int LoginId = int.Parse(ConfigurationManager.AppSettings["LoginId"]);
        }
    }
}
