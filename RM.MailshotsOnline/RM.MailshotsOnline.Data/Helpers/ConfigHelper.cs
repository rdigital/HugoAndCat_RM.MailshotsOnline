using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Helpers
{
    public class ConfigHelper
    {
        /// <summary>
        /// Umbraco ID of the Formats folder
        /// </summary>
        public static int FormatsFolderId
        {
            get { return GetConfigValue("FormatsFolderId", 1056); }
        }

        /// <summary>
        /// Umbraco ID of the Layouts folder
        /// </summary>
        public static int LayoutsFolderId
        {
            get { return GetConfigValue("LayoutsFolderId", 1057); }
        }

        /// <summary>
        /// Umbraco ID of the Themes folder
        /// </summary>
        public static int ThemesFolderId
        {
            get { return GetConfigValue("ThemesFolderId", 1173); }
        }

        private static int GetConfigValue(string key, int defaultValue)
        {
            int result;
            if (!int.TryParse(GetConfigValue(key), out result))
            {
                return defaultValue;
            }

            return result;
        }

        private static string GetConfigValue(string key)
        {
            return CloudConfigurationManager.GetSetting(key) ?? ConfigurationManager.AppSettings[key];
        }
    }
}
