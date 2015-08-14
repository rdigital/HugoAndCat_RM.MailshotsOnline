using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.JsonModels;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class MediaExtensions
    {
        public static Umbraco.Core.Models.IMedia SetValues(this Umbraco.Core.Models.IMedia umbracoMedia, PrivateLibraryImage media)
        {
            umbracoMedia.SetValue("originalBlobUrl", media.OriginalBlobId);
            umbracoMedia.SetValue("smallThumbBlobId", media.SmallThumbBlobId);
            umbracoMedia.SetValue("largeThumbBlobId", media.LargeThumbBlobId);

            umbracoMedia.SetValue("originalUrl", media.OriginalUrl);
            umbracoMedia.SetValue("smallThumbUrl", media.SmallThumbUrl);
            umbracoMedia.SetValue("largeThumbUrl", media.LargeThumbUrl);

            return umbracoMedia;
        }
    }
}
