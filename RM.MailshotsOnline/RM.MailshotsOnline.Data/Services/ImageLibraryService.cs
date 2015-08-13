using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glass.Mapper.Umb;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Data.Media_Conversion;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core;
using Umbraco.Web;
using IMember = RM.MailshotsOnline.PCL.Models.IMember;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;
using UmbracoContext = Umbraco.Web.UmbracoContext;

namespace RM.MailshotsOnline.Data.Services
{
    public class ImageLibraryService : IImageLibraryService
    {
        private readonly UmbracoHelper _helper = new UmbracoHelper(UmbracoContext.Current);
        private readonly BlobStorageHelper _blobStorage =
            new BlobStorageHelper(ConfigHelper.StorageConnectionString, ConfigHelper.PrivateMediaBlobStorageContainer);
        private readonly ILogger _logger;

        public ImageLibraryService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get a list of all tags stored in Umbraco.
        /// </summary>
        /// <returns>The list of all tags.</returns>
        public IEnumerable<string> GetTags()
        {
            return _helper.TagQuery.GetAllTags(ContentConstants.Settings.DefaultMediaLibraryTagGroup)
                .Select(x => x.Text);
        }

        /// <summary>
        /// Get all images in the public library.
        /// </summary>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages()
        {
            return
                _helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup)
                    .Select(x => MediaFactory.Convert(x, typeof (PublicLibraryImage)));
        }

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages(string tag)
        {
            return _helper.TagQuery.GetMediaByTag(tag, ContentConstants.Settings.DefaultMediaLibraryTagGroup).Select(x => MediaFactory.Convert(x, typeof(PublicLibraryImage)));
        }

        /// <summary>
        /// Get all images belonging to the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages(IMember member)
        {
            var privateImages =
                _helper.ContentQuery.TypedMedia(ContentConstants.MediaContent.PrivateMediaLibraryId)
                    .DescendantsOrSelf(ContentConstants.MediaContent.PrivateLibraryImageMediaTypeAlias);

            var memberPrivateImages =
                privateImages.Where(x => x.GetPropertyValue("username").Equals(member.Username.ToString()));

            return memberPrivateImages.Select(x => MediaFactory.Convert(x, typeof(PrivateLibraryImage)));
        }

        public bool AddImage(byte[] bytes, string name, string extension, IMember member)
        {
            var filename = $"{member.Id}/{name}{DateTime.UtcNow}.{extension}";

            try
            {
                _blobStorage.StoreBytes(bytes, filename, $"image/{extension.ToLower()}");
            }
            catch(Exception e)
            {
                _logger.Error(this.GetType().Name, "AddImage", $"{e.GetType().Name} {e.Message} {e.StackTrace}");

                return false;
            }

            return true;
        }

        public bool DeleteImage(IMedia image)
        {
            throw new NotImplementedException();
        }

        public bool RenameImage(IMedia image, string name)
        {
            throw new NotImplementedException();
        }
    }
}