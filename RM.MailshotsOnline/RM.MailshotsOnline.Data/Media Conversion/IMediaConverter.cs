using Umbraco.Core.Models;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;

namespace RM.MailshotsOnline.Data.Media_Conversion
{
    public interface IMediaConverter
    {
        /// <summary>
        /// Convert the given content iten into an IMedia
        /// </summary>
        /// <param name="content">The content item</param>
        /// <param name="o">The object that </param>
        /// <returns></returns>
        IMedia Convert(IPublishedContent content, object o);
    }
}