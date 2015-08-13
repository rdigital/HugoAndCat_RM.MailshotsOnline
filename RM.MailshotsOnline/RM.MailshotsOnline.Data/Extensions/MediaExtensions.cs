using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class MediaExtensions
    {
        public static Image SetValues(this Image image, IPublishedContent content)
        {
            image.Src = content.GetPropertyValue("umbracoFile")?.ToString();
            image.Width = content.GetPropertyValue("umbracoWidth")?.ToString();
            image.Height = content.GetPropertyValue("umbracoHeight")?.ToString();
            image.Size = content.GetPropertyValue("umbracoBytes")?.ToString();
            image.Type = content.GetPropertyValue("umbracoExtension")?.ToString();
            image.Name = content.GetPropertyValue("umbracoName")?.ToString();

            return image;
        }
    }
}
