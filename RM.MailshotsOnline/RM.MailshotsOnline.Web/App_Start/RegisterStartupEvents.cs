﻿using log4net;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Data.Migrations;
using RM.MailshotsOnline.Data.Services;
using RM.MailshotsOnline.Entities.DataModels;
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
using Umbraco.Web.UI.Pages;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterStartupEvents : ApplicationEventHandler
    {
        private IMailshotSettingsService _settingsService;
        private ICmsImageService _cmsImageService;

        /// <summary>
        /// Runs immediately after the Application Start for Umbraco.  Register new events here
        /// </summary>
        /// <see cref="https://our.umbraco.org/Documentation/Reference/Events-v6/Application-Startup"/>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Created += ContentService_Created;
            ContentService.Saving += ContentService_Saving;
            ContentService.Saved += ContentService_Saved;
            ContentService.Trashing += ContentService_Trashing;
            ContentService.Deleting += ContentService_Deleting;
        }

        #region Event handlers

        /// <summary>
        /// Run before the content service can move an item to the trash
        /// </summary>
        private void ContentService_Trashing(IContentService sender, Umbraco.Core.Events.MoveEventArgs<IContent> e)
        {
            foreach (var moveAction in e.MoveInfoCollection)
            {
                if (ContentIsImageLibraryItem(moveAction.Entity))
                {
                    if (ContentIsUsedImage(moveAction.Entity))
                    {
                        e.Cancel = true;
                        ((BasePage)HttpContext.Current.Handler).ClientTools.ShowSpeechBubble(Umbraco.Web.UI.SpeechBubbleIcon.Error, "Unable to delete", "The image you tried to delete is being used in a Mailshot so can't be deleted.");
                    }
                    else
                    {
                        DeleteCmsImage(moveAction.Entity);
                    }
                }
            }
        }

        /// <summary>
        /// Run before the content service can delete an item
        /// </summary>
        private void ContentService_Deleting(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<IContent> e)
        {
            foreach (var item in e.DeletedEntities)
            {
                if (ContentIsImageLibraryItem(item))
                {
                    if (ContentIsUsedImage(item))
                    {
                        e.Cancel = true;
                        ((BasePage)HttpContext.Current.Handler).ClientTools.ShowSpeechBubble(Umbraco.Web.UI.SpeechBubbleIcon.Error, "Unable to delete", "The image you tried to delete is being used in a Mailshot so can't be deleted.");
                    }
                    else
                    {
                        DeleteCmsImage(item);
                    }
                }
            }
        }

        /// <summary>
        /// Run after the content service has saved an item
        /// </summary>
        void ContentService_Saved(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
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
                else if (item.ContentType.Alias.InvariantEquals(ConfigHelper.PublicLibraryImageContentTypeAlias) || item.ContentType.Alias.InvariantEquals(ConfigHelper.PrivateImageContentTypeAlias))
                {
                    SaveCmsImage(item);
                }
            }
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

        #endregion

        #region Private methods

        private void DeleteCmsImage(IContent item)
        {
            // Confirm we have CMS image service object
            _cmsImageService = _cmsImageService ?? new CmsImageService();

            // Delete the item
            _cmsImageService.DeleteCmsImage(item.Id);

            //TODO: Remove the image blob
        }

        /// <summary>
        /// Saves a CMS image to the DB
        /// </summary>
        private void SaveCmsImage(IContent item)
        {
            // Confirm we have CMS image service object
            _cmsImageService = _cmsImageService ?? new CmsImageService();

            CmsImage cmsImage = null;
            _cmsImageService.GetCmsImage(item.Id);
            if (cmsImage == null)
            {
                cmsImage = new CmsImage
                {
                    UmbracoMediaId = item.Id,
                    Src = item.GetValue<string>("umbracoFile")
                };

                if (item.HasProperty("userId"))
                {
                    cmsImage.UserId = item.GetValue<int>("userId");
                }
            }
            else
            {
                cmsImage.Src = item.GetValue<string>("umbracoFile");
                if (item.HasProperty("userId"))
                {
                    cmsImage.UserId = item.GetValue<int>("userId");
                }
                else
                {
                    cmsImage.UserId = null;
                }
            }

            _cmsImageService.SaveCmsImage(cmsImage);
        }

        private bool ContentIsImageLibraryItem(IContent item)
        {
            return item.ContentType.Alias == ConfigHelper.PublicLibraryImageContentTypeAlias || item.ContentType.Alias == ConfigHelper.PrivateImageContentTypeAlias;
        }

        /// <summary>
        /// Checks to see if the given item is a CMS image that's used in a mailshot
        /// </summary>
        private bool ContentIsUsedImage(IContent item)
        {
            var contentIsUsed = false;
            if (item.ContentType.Alias == ConfigHelper.PublicLibraryImageContentTypeAlias || item.ContentType.Alias == ConfigHelper.PrivateImageContentTypeAlias)
            {
                // Ensure we have a CMS Image Service
                _cmsImageService = _cmsImageService ?? new CmsImageService();

                if (_cmsImageService.IsImageUsedInMailshot(item.Id))
                {
                    contentIsUsed = true;
                }
            }

            return contentIsUsed;
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

        /// <summary>
        /// Saves a Format item to the DB
        /// </summary>
        private void SaveFormat(IContent item)
        {
            _settingsService = _settingsService ?? new MailshotSettingsService();

            var format = new Format()
            {
                UmbracoPageId = item.Id,
                Name = item.Name,
                XslData = item.GetValue<string>("xslData"),
                JsonIndex = item.GetValue<int>("jsonIndex"),
                UpdatedDate = DateTime.UtcNow
            };

            _settingsService.AddOrUpdateFormat(format);
        }

        /// <summary>
        /// Saves a Template item to the DB
        /// </summary>
        private void SaveTemplate(IContent item)
        {
            _settingsService = _settingsService ?? new MailshotSettingsService();

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

        /// <summary>
        /// Saves a Theme item to the DB
        /// </summary>
        private void SaveTheme(IContent item)
        {
            _settingsService = _settingsService ?? new MailshotSettingsService();

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

        #endregion
    }
}