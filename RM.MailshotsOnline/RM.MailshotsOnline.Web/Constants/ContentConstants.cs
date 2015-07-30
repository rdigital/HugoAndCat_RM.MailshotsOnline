using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Constants
{
    public static class ContentConstants
    {
        public static class Settings
        {
            public static readonly int HeaderNavSettingsId = int.Parse(ConfigurationManager.AppSettings["HeaderNavSettingsId"]);
        }

        public static class HomeContent
        {
            public static readonly int LoginId = int.Parse(ConfigurationManager.AppSettings["LoginId"]);
        }
    }
}
