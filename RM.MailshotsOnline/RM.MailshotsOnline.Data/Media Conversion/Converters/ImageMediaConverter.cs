using RM.MailshotsOnline.Entities.JsonModels;
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
            Media.Name = content.GetPropertyValue("contentName")?.ToString();

            return Media;
        }
    }
}