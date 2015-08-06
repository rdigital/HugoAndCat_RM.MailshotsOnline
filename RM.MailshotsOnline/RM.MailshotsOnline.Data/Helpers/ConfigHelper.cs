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
        /// Content Type Alias for the Mailshot Format items
        /// </summary>
        public static string FormatContentTypeAlias
        {
            get { return GetConfigValue("FormatTemplateAlias", "Format"); }
        }

        /// <summary>
        /// Content Type Alias for the Mailshot Template items
        /// </summary>
        public static string TemplateContentTypeAlias
        {
            get { return GetConfigValue("TemplateContentTypeAlias", "Layout"); }
        }

        /// <summary>
        /// Content Type Alias for the Mailshot Theme items
        /// </summary>
        public static string ThemeContentTypeAlias
        {
            get { return GetConfigValue("ThemeContentTypeAlias", "Theme"); }
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

        private static string GetConfigValue(string key, string defaultValue)
        {
            return GetConfigValue(key) ?? defaultValue;
        }

        private static string GetConfigValue(string key)
        {
            return CloudConfigurationManager.GetSetting(key) ?? ConfigurationManager.AppSettings[key];
        }
    }
}
