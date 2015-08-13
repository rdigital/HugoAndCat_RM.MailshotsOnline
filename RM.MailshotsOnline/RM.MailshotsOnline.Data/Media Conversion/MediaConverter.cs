using Umbraco.Core.Models;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion
{
    public abstract class MediaConverter<T> : IMediaConverter where T : IMedia
    {
        public abstract T Media { get; set; }

        public abstract IMedia Convert(IPublishedContent content, object o);
    }
}