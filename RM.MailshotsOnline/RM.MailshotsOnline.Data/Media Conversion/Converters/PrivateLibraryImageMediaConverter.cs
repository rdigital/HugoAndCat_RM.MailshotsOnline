using System;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models;
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
            Media.OriginalBlobId = content.GetPropertyValue("originalBlobId")?.ToString();
            Media.SmallThumbBlobId = content.GetPropertyValue("smallThumbBlobId")?.ToString();
            Media.LargeThumbBlobId = content.GetPropertyValue("largeThumbBlobId")?.ToString();
            Media.OriginalUrl = content.GetPropertyValue("originalUrl")?.ToString();
            Media.SmallThumbUrl = content.GetPropertyValue("smallThumbUrl")?.ToString();
            Media.LargeThumbUrl = content.GetPropertyValue("largeThumbUrl")?.ToString();

            return Media;
        }

        public override IMedia Convert(Umbraco.Core.Models.IMedia content, object o)
        {
            Media = (PrivateLibraryImage)o;
            Media.Username = content.GetValue("username")?.ToString();
            Media.OriginalBlobId = content.GetValue("originalBlobId")?.ToString();
            Media.SmallThumbBlobId = content.GetValue("smallThumbBlobId")?.ToString();
            Media.LargeThumbBlobId = content.GetValue("largeThumbBlobId")?.ToString();
            Media.OriginalUrl = content.GetValue("originalUrl")?.ToString();
            Media.SmallThumbUrl = content.GetValue("smallThumbUrl")?.ToString();
            Media.LargeThumbUrl = content.GetValue("largeThumbUrl")?.ToString();

            return Media;
        }
    }
}