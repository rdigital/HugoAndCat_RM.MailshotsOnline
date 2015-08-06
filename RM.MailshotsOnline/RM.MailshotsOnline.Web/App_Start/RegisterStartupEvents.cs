using log4net;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Data.Migrations;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.DataModels.MailshotSettings;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterStartupEvents : ApplicationEventHandler
    {
        private IMailshotSettingsService _settingsService;

        //public RegisterStartupEvents(IMailshotSettingsService settingsService) : base()
        //{
        //    _settingsService = settingsService;
        //}

        /// <summary>
        /// Runs immediately after the Application Start for Umbraco.  Register new events here
        /// </summary>
        /// <see cref="https://our.umbraco.org/Documentation/Reference/Events-v6/Application-Startup"/>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            var dbMigrator = new DbMigrator(new Configuration());
            dbMigrator.Update();

            ContentService.Created += ContentService_Created;
            ContentService.Saving += ContentService_Saving;
            ContentService.Saved += ContentService_Saved;
        }

        /// <summary>
        /// Run after the content service has saved an item
        /// </summary>
        void ContentService_Saved(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            //TODO: Get this properly with DI
            var settingsAliases = new List<string>()
            {
                ConfigHelper.FormatContentTypeAlias,
                ConfigHelper.TemplateContentTypeAlias,
                ConfigHelper.ThemeContentTypeAlias
            };

            if (e.SavedEntities.Any(i => settingsAliases.Contains(i.ContentType.Alias)))
            {
                _settingsService = new MailshotSettingsService();
            }

            foreach (var item in e.SavedEntities)
            {
                // Can't use switch statement as the aliases need to come from configuration variables
                if (item.ContentType.Alias.InvariantEquals(ConfigHelper.FormatContentTypeAlias))
                {
                    SaveFormat(item);
                }
                else if (item.ContentType.Alias.InvariantEquals(ConfigHelper.TemplateContentTypeAlias))
                {
                    SaveTemplate(item);
                }
                else if (item.ContentType.Alias.InvariantEquals(ConfigHelper.ThemeContentTypeAlias))
                {
                    SaveTheme(item);
                }
            }
        }

        private void SaveFormat(IContent item)
        {
            var  format = new Format()
            {
                UmbracoPageId = item.Id,
                Name = item.Name,
                XslData = item.GetValue<string>("xslData"),
                JsonIndex = item.GetValue<int>("jsonIndex"),
                UpdatedDate = DateTime.UtcNow
            };

            _settingsService.AddOrUpdateFormat(format);
        }

        private void SaveTemplate(IContent item)
        {
            var template = new Entities.DataModels.MailshotSettings.Template()
            {
                UmbracoPageId = item.Id,
                Name = item.Name,
                XslData = item.GetValue<string>("xslData"),
                JsonIndex = item.GetValue<int>("jsonIndex"),
                FormatUmbracoPageId = item.GetValue<int>("format"),
                UpdatedDate = DateTime.UtcNow
            };

            _settingsService.AddOrUpdateTemplate(template);
        }

        private void SaveTheme(IContent item)
        {
            var theme = new Theme()
            {
                UmbracoPageId = item.Id,
                Name = item.Name,
                XslData = item.GetValue<string>("xslData"),
                JsonIndex = item.GetValue<int>("jsonIndex"),
                UpdatedDate = DateTime.UtcNow
            };

            _settingsService.AddOrUpdateTheme(theme);
        }

        /// <summary>
        /// Run before the content service saves items
        /// </summary>
        void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            foreach (var item in e.SavedEntities)
            {
                // If the item doesn't have an identity, it's new
                if (!item.HasIdentity)
                {
                    SetStringIfEmpty(item, "metaPageTitle", item.Name);
                    SetStringIfEmpty(item, "navigationTitle", item.Name);
                }
            }
        }

        /// <summary>
        /// Run after the content service has created an item
        /// </summary>
        void ContentService_Created(IContentService sender, Umbraco.Core.Events.NewEventArgs<IContent> e)
        {
            var item = e.Entity;

            // If the item doesn't have an identity, it's new
            if (!item.HasIdentity)
            {
                SetPropertyValue(item, "displayInNavigation", true);
            }
        }

        /// <summary>
        /// Sets a string value for an item if it hasn't already been set
        /// </summary>
        private void SetStringIfEmpty(IContent item, string propertyName, string value)
        {
            if (item.HasProperty(propertyName))
            {
                if (string.IsNullOrEmpty(item.GetValue<string>(propertyName)))
                {
                    item.SetValue(propertyName, value);
                }
            }
        }

        /// <summary>
        /// Sets a property value for an item
        /// </summary>
        private void SetPropertyValue(IContent item, string propertyName, object value)
        {
            if (item.HasProperty(propertyName))
            {
                item.SetValue(propertyName, value);
            }
        }
    }
}