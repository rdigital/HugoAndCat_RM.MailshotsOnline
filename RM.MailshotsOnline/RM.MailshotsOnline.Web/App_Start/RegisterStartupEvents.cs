using HC.RM.Common.Azure;
using HC.RM.Common.PCL.Helpers;
using log4net;
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
        /// Runs as the application is starting
        /// </summary>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //System.Web.Http.GlobalConfiguration.Configuration.MessageHandlers.Add(new UmbracoMessageHandler());
        }

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
            MediaService.Saved += MediaService_Saved;
            MediaService.Trashing += MediaService_Trashing;
            MediaService.Deleting += MediaService_Deleting;
        }

        #region Event handlers

        /// <summary>
        /// Runs before a media item is deleted
        /// </summary>
        private void MediaService_Deleting(IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<IMedia> e)
        {
            var cancelled = false;
            foreach (var item in e.DeletedEntities)
            {
                if (MediaIsImageLibraryItem(item))
                {
                    if (MediaIsUsedImage(item))
                    {
                        e.Cancel = true;
                        cancelled = true;
                        //((BasePage)HttpContext.Current.Handler).ClientTools.ShowSpeechBubble(Umbraco.Web.UI.SpeechBubbleIcon.Error, "Unable to delete", "The image you tried to delete is being used in a Mailshot so can't be deleted.");
                    }
                    else
                    {
                        DeleteCmsImage(item);
                    }
                }
            }

            if (cancelled)
            {
                throw new Exception("Unable to delete image that is still being used.");
                // TODO: Do this a better way using some sort of feedback to the CMS user
            }
        }

        /// <summary>
        /// Runs before a media item is moved to the trash
        /// </summary>
        private void MediaService_Trashing(IMediaService sender, Umbraco.Core.Events.MoveEventArgs<IMedia> e)
        {
            var cancelled = false;
            foreach (var moveAction in e.MoveInfoCollection)
            {
                if (MediaIsImageLibraryItem(moveAction.Entity))
                {
                    if (MediaIsUsedImage(moveAction.Entity))
                    {
                        e.Cancel = true;
                        cancelled = true;
                        //((BasePage)HttpContext.Current.Handler).ClientTools.ShowSpeechBubble(Umbraco.Web.UI.SpeechBubbleIcon.Error, "Unable to delete", "The image you tried to delete is being used in a Mailshot so can't be deleted.");
                    }
                    else
                    {
                        //DeleteCmsImage(moveAction.Entity);
                    }
                }
            }

            if (cancelled)
            {
                throw new Exception("Unable to delete image that is still being used.");
                // TODO: Do this a better way using some sort of feedback to the CMS user
            }
        }

        /// <summary>
        /// Runs after a media item is saved
        /// </summary>
        private void MediaService_Saved(IMediaService sender, Umbraco.Core.Events.SaveEventArgs<IMedia> e)
        {
            foreach (var item in e.SavedEntities)
            {
                if (item.ContentType.Alias.InvariantEquals(ConfigHelper.PublicLibraryImageContentTypeAlias) || item.ContentType.Alias.InvariantEquals(ConfigHelper.PrivateImageContentTypeAlias))
                {
                    SaveCmsImage(item);
                }
            }
        }

        /// <summary>
        /// Run before the content service can move an item to the trash
        /// </summary>
        private void ContentService_Trashing(IContentService sender, Umbraco.Core.Events.MoveEventArgs<IContent> e)
        {
            
        }

        /// <summary>
        /// Run before the content service can delete an item
        /// </summary>
        private void ContentService_Deleting(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<IContent> e)
        {
            
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

        private void DeleteCmsImage(IMedia item)
        {
            // Confirm we have CMS image service object
            _cmsImageService = _cmsImageService ?? new CmsImageService();

            // Delete the item
            _cmsImageService.DeleteCmsImage(item.Id);

            // Remove the image blobs
            if (item.ContentType.Alias == ConfigHelper.PrivateImageContentTypeAlias)
            {
                BlobStorageHelper blobStorage = new BlobStorageHelper(ConfigHelper.PrivateStorageConnectionString, ConfigHelper.PrivateMediaBlobStorageContainer);
                TryDeleteBlob(blobStorage, item.GetValue<string>("originalBlobId"));
                TryDeleteBlob(blobStorage, item.GetValue<string>("smallThumbBlobId"));
                TryDeleteBlob(blobStorage, item.GetValue<string>("largeThumbBlobId"));
            }
        }

        private void TryDeleteBlob(BlobStorageHelper helper, string blobId)
        {
            try
            {
                helper.DeleteBlob(blobId);
            }
            catch (Exception ex)
            {
                ILogger log = new Logger();
                log.Exception(this.GetType().Name, "TryDeleteBlob", ex);
                log.Error(this.GetType().Name, "TryDeleteBlob", "Unable to delete blobg with ID {0}", blobId);
            }
        }

        /// <summary>
        /// Saves a CMS image to the DB
        /// </summary>
        private void SaveCmsImage(IMedia item)
        {
            // Confirm we have CMS image service object
            _cmsImageService = _cmsImageService ?? new CmsImageService();

            CmsImage cmsImage = (CmsImage)_cmsImageService.GetCmsImage(item.Id);
            if (cmsImage == null)
            {
                cmsImage = new CmsImage
                {
                    UmbracoMediaId = item.Id,
                    Src = item.GetValue<string>("umbracoFile"),
                    UpdatedDate = DateTime.UtcNow
                };

                if (item.HasProperty("username"))
                {
                    cmsImage.UserName = item.GetValue<string>("username");
                }

                if (item.HasProperty("originalUrl"))
                {
                    cmsImage.Src = item.GetValue<string>("originalUrl");
                }
            }
            else
            {
                cmsImage.Src = item.GetValue<string>("umbracoFile");
                cmsImage.UpdatedDate = DateTime.UtcNow;
                if (item.HasProperty("username"))
                {
                    cmsImage.UserName = item.GetValue<string>("username");
                }
                else
                {
                    cmsImage.UserName = null;
                }

                if (item.HasProperty("originalUrl"))
                {
                    cmsImage.Src = item.GetValue<string>("originalUrl");
                }
            }

            _cmsImageService.SaveCmsImage(cmsImage);
        }

        private bool MediaIsImageLibraryItem(IMedia item)
        {
            return item.ContentType.Alias == ConfigHelper.PublicLibraryImageContentTypeAlias || item.ContentType.Alias == ConfigHelper.PrivateImageContentTypeAlias;
        }

        /// <summary>
        /// Checks to see if the given item is a CMS image that's used in a mailshot
        /// </summary>
        private bool MediaIsUsedImage(IMedia item)
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