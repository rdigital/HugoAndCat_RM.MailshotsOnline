using System;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion.Converters
{
    public class ImageMediaConverter : MediaConverter<Image>
    {
        public override Image Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (Image)o;
            Media.Src = content.GetPropertyValue("umbracoFile")?.ToString();
            Media.Width = content.GetPropertyValue("umbracoWidth")?.ToString();
            Media.Height = content.GetPropertyValue("umbracoHeight")?.ToString();
            Media.Size = content.GetPropertyValue("umbracoBytes")?.ToString();
            Media.Type = content.GetPropertyValue("umbracoExtension")?.ToString();

            return Media;
        }

        public override IMedia Convert(Umbraco.Core.Models.IMedia content, object o)
        {
            Media = (Image)o;
            Media.Src = content.GetValue("umbracoFile")?.ToString();
            Media.Width = content.GetValue("umbracoWidth")?.ToString();
            Media.Height = content.GetValue("umbracoHeight")?.ToString();
            Media.Size = content.GetValue("umbracoBytes")?.ToString();
            Media.Type = content.GetValue("umbracoExtension")?.ToString();

            return Media;
        }
    }
}