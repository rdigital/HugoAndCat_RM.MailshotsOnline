using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace RM.MailshotsOnline.Web.App_Start
{
    public class RegisterPageEvents : ApplicationEventHandler
    {
        /// <summary>
        /// Runs immediately after the Application Start for Umbraco.  Register new events here
        /// </summary>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Created += ContentService_Created;
            ContentService.Saving += ContentService_Saving;
        }

        void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            foreach (var item in e.SavedEntities)
            {
                // If the item doesn't have an identity, it's new
                if (!item.HasIdentity)
                {
                    SetDefaultString(item, "metaPageTitle", item.Name);
                    SetDefaultString(item, "navigationTitle", item.Name);
                }
            }
        }

        void ContentService_Created(IContentService sender, Umbraco.Core.Events.NewEventArgs<IContent> e)
        {
            var item = e.Entity;

            // If the item doesn't have an identity, it's new
            if (!item.HasIdentity)
            {
                SetDefaultBool(item, "displayInNavigation", true);
            }
        }

        /// <summary>
        /// Sets a default string for an item
        /// </summary>
        private void SetDefaultString(IContent item, string propertyName, string value)
        {
            if (item.Properties.Contains(propertyName))
            {
                if (item.Properties[propertyName].Value == null)
                {
                    item.Properties[propertyName].Value = value;
                }
                else if (string.IsNullOrEmpty(item.Properties[propertyName].Value.ToString()))
                {
                    item.Properties[propertyName].Value = value;
                }
            }
        }

        /// <summary>
        /// Sets a default bool value for an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private void SetDefaultBool(IContent item, string propertyName, bool value)
        {
            if (item.Properties.Contains(propertyName))
            {
                item.Properties[propertyName].Value = value;
            }
        }
    }
}