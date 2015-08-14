using RM.MailshotsOnline.Entities.JsonModels;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;
using Media = RM.MailshotsOnline.Entities.JsonModels.Media;

namespace RM.MailshotsOnline.Data.Media_Conversion.Converters
{
    public class MediaMediaConverter : MediaConverter<Media>
    {
        public override Media Media { get; set; }

        public override IMedia Convert(IPublishedContent content, object o)
        {
            Media = (Media)o;
            Media.Name = content.Name;
            Media.MailshotUses = 0; // Filled in by another service
            Media.MediaId = content.Id;

            return Media;
        }
    }
}
