using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion.Converters
{
    public class PublicLibraryImageMediaConverter : MediaConverter<PublicLibraryImage>
    {
        public override PublicLibraryImage Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (PublicLibraryImage)o;
            Media.Tags = content.GetPropertyValue("tags")?.ToString().Split(',');

            return Media;
        }
    }
}