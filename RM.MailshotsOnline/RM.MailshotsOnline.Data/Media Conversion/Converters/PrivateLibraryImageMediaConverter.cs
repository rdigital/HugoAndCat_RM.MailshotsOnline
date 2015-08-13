using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion.Converters
{
    public class PrivateLibraryImageMediaConverter : MediaConverter<PrivateLibraryImage>
    {
        public override PrivateLibraryImage Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (PrivateLibraryImage)o;
            Media.Username = content.GetPropertyValue("username")?.ToString();

            return Media;
        }
    }
}