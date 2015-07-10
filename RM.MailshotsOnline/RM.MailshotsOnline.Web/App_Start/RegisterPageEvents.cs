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
        /// <see cref="https://our.umbraco.org/Documentation/Reference/Events-v6/Application-Startup"/>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Created += ContentService_Created;
            ContentService.Saving += ContentService_Saving;
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