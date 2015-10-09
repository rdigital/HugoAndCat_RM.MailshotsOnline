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
        /// The Application Insights key
        /// </summary>
        public static string AppInsightsKey
        {
            get { return GetConfigValue("AppInsightsKey", "f4926bc4-a07f-420c-89f1-40e38682d0c6"); }
        }

        public static string CurrencyCode
        {
            get { return GetConfigValue("CurrencyCode", "GBP"); }
        }

        /// <summary>
        /// The JSON ID of the default mailshot content
        /// </summary>
        public static int DefaultMailshotContentJsonIndex
        {
            get { return GetConfigValue("DefaultMailshotContentJsonIndex", 1); }
        }

        /// <summary>
        /// Content Type Alias for the Mailshot Format items
        /// </summary>
        public static string FormatContentTypeAlias
        {
            get { return GetConfigValue("FormatTemplateAlias", "Format"); }
        }

        public static string HostedDomain
        {
            get { return GetConfigValue("HostedDomain"); }
        }

        public static string HostedPort
        {
            get { return GetConfigValue("HostedPort"); }
        }

        public static string HostedScheme
        {
            get { return GetConfigValue("HostedScheme"); }
        }

        public static string MailshotContentBlobContainer
        {
            get { return GetConfigValue("MailshotContentBlobContainer", "mailshotcontent"); }
        }

        public static string MailshotDefaultContentTypeAlias
        {
            get { return GetConfigValue("MailshotDefaultContentTypeAlias", "MailshotDefaultContent"); }
        }

        public static string RoyalMailApprovalEmailAddress
        {
            get { return GetConfigValue("RoyalMailApprovalEmailAddress"); }
        }

        public static bool SaveMailshotInfoForDebug
        {
            get { return GetConfigValue("SaveMailshotInfoForDebug", false); }
        }

        public static string SettingsFolderContentTypeAlias
        {
            get { return GetConfigValue("SettingsFolderContentTypeAlias", "SettingsFolder"); }
        }

        public static string SparqServiceBlobContainer
        {
            get { return GetConfigValue("SparqServiceBlobContainer"); }
        }

        public static string SparqServiceBlobConnectionString
        {
            get { return GetConfigValue("SparqServiceBlobConnectionString"); }
        }

        public static string SparqServiceBusConnectionString
        {
            get { return GetConfigValue("SparqServiceBusConnectionString"); }
        }

        public static string SparqBlobContainer
        {
            get { return GetConfigValue("SparqBlobContainer"); }
        }

        public static string StorageConnectionString
        {
            get { return GetConfigValue("StorageConnectionString"); }
        }

        public static string SparqServiceIpRangeStart
        {
            get { return GetConfigValue("SparqServiceIpRangeStart"); }
        }

        public static string SparqServiceIpRangeEnd
        {
            get { return GetConfigValue("SparqServiceIpRangeEnd"); }
        }

        public static string StIvesPrintNotificationAddress
        {
            get { return GetConfigValue("StIvesPrintNotificationAddress"); }
        }

        /// <summary>
        /// The System "From" email address
        /// </summary>
        public static string SystemEmailAddress
        {
            get { return GetConfigValue("SystemEmailAddress"); }
        }

        /// <summary>
        /// Content Type Alias for Postal Option items
        /// </summary>
        public static string PostalOptionContentTypeAlias
        {
            get { return GetConfigValue("PostalOptionContentTypeAlias", "PostageOption"); }
        }

        /// <summary>
        /// Content Type Alias for the Private Image items
        /// </summary>
        public static string PrivateImageContentTypeAlias
        {
            get { return GetConfigValue("PrivateImageContentTypeAlias", "PrivateLibraryImage"); }
        }

        /// <summary>
        /// The Blob Storage container name for the Private Media container
        /// </summary>
        public static string PrivateMediaBlobStorageContainer
        {
            get { return GetConfigValue("PrivateMediaBlobStorageContainer"); }
        }

        /// <summary>
        /// Connection string for the Private blob storage container
        /// </summary>
        public static string PrivateStorageConnectionString
        {
            get { return GetConfigValue("PrivateStorageConnectionString"); }
        }

        /// <summary>
        /// Content Type Alias for Public Library Image
        /// </summary>
        public static string PublicLibraryImageContentTypeAlias
        {
            get { return GetConfigValue("PublicLibraryImageContentTypeAlias", "PublicLibraryImage"); }
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

        internal static bool GetConfigValue(string key, bool defaultValue)
        {
            bool result;
            if (!bool.TryParse(GetConfigValue(key), out result))
            {
                return defaultValue;
            }

            return result;
        }

        internal static int GetConfigValue(string key, int defaultValue)
        {
            int result;
            if (!int.TryParse(GetConfigValue(key), out result))
            {
                return defaultValue;
            }

            return result;
        }

        internal static decimal GetConfigValue(string key, decimal defaultValue)
        {
            decimal result;
            if (!decimal.TryParse(GetConfigValue(key), out result))
            {
                return defaultValue;
            }

            return result;
        }

        internal static string GetConfigValue(string key, string defaultValue)
        {
            return GetConfigValue(key) ?? defaultValue;
        }

        internal static string GetConfigValue(string key)
        {
            return CloudConfigurationManager.GetSetting(key) ?? ConfigurationManager.AppSettings[key];
        }
    }
}
