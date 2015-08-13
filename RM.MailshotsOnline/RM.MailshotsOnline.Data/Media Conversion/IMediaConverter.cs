using Umbraco.Core.Models;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion
{
    public interface IMediaConverter
    {
        IMedia Convert(IPublishedContent content, object o);
    }
}