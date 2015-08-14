using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Glass.Mapper.Umb;
using HC.RM.Common.Images;
using HC.RM.Common.PCL.Helpers;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Data.Media_Conversion;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core;
using Umbraco.Core.Services;
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
        private readonly ICmsImageService _cmsImageService;
        private readonly IMediaService _mediaService;
        private readonly ImageResizer _imageResizer = new ImageResizer();

        public ImageLibraryService(ILogger logger, ICmsImageService cmsImageService, IMediaService mediaService)

        {
            _logger = logger;
            _cmsImageService = cmsImageService;
            _mediaService = mediaService;
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
            return PopulateUsageCounts(_helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup).Select(x => MediaFactory.Convert(x, typeof(PublicLibraryImage))));
        }

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages(string tag)
        {

            return PopulateUsageCounts(_helper.TagQuery.GetMediaByTag(tag, ContentConstants.Settings.DefaultMediaLibraryTagGroup).Select(x => MediaFactory.Convert(x, typeof(PublicLibraryImage))));
        }

        public IMedia GetImage(int mediaId, bool publicImage)
        {
            var requiredType = publicImage ? typeof(PublicLibraryImage) : typeof(PrivateLibraryImage);
            IMedia image = MediaFactory.Convert(_helper.TypedMedia(mediaId), requiredType);
            image.MailshotUses = _cmsImageService.GetImageUsageCount(mediaId);
            return image;
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


            return PopulateUsageCounts(memberPrivateImages.Select(x => MediaFactory.Convert(x, typeof(PrivateLibraryImage))));
        }


        /// <summary>
        /// Add an image into the blob store for the given member.
        /// </summary>
        /// <param name="bytes">THe byte representing the image.</param>
        /// <param name="name">The name of the image.</param>
        /// <param name="member">The member creating this image. The image will be stored in this member's personal image store.</param>
        /// <returns></returns>
        public IMedia AddImage(byte[] bytes, string name, IMember member)
        {
            byte[] smallThumb;
            byte[] largeThumb;
            System.Drawing.Image original;
            try
            {
                original = _imageResizer.GetImage(bytes);
                smallThumb = _imageResizer.ResizeImageBytes(original, ContentConstants.Settings.ImageThumbnailSizeSmall);
                largeThumb = _imageResizer.ResizeImageBytes(original, ContentConstants.Settings.ImageThumbnailSizeLarge);
            }
            catch (Exception e)
            {
                _logger.Error(this.GetType().Name, "AddImage", $"{e.GetType().Name} {e.Message} {e.StackTrace}");

                return null;
            }

            var extension = original.RawFormat.ToString().ToLower();
            var originalFilename = $"{member.Id}/{name}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.{extension.ToLower()}";
            var smallThumbFilename = $"{member.Id}/{name}-thumbSmall-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.{extension.ToLower()}";
            var largeThumbFilename = $"{member.Id}/{name}-thumbLarge-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}.{extension.ToLower()}";

            try
            {
                _blobStorage.StoreBytes(bytes, originalFilename, $"image/{extension.ToLower()}");
                _blobStorage.StoreBytes(smallThumb, smallThumbFilename, $"image/{extension.ToLower()}");
                _blobStorage.StoreBytes(largeThumb, largeThumbFilename, $"image/{extension.ToLower()}");
            }
            catch (Exception e)
            {
                _logger.Error(this.GetType().Name, "AddImage", $"{e.GetType().Name} {e.Message} {e.StackTrace}");

                return null;
            }

            var memberMediaFolder =
                _helper.ContentQuery.TypedMedia(ContentConstants.MediaContent.PrivateMediaLibraryId)
                    .Children.FirstOrDefault(x => x.Name.Equals(member.Username))?.Id;

            if (memberMediaFolder == null)
            {
                memberMediaFolder = _mediaService.CreateMedia(member.Username, ContentConstants.MediaContent.PrivateMediaLibraryId,
                    ContentConstants.MediaContent.PrivateImageLibraryFolderMediaTypeAlias).Id;
            }

            var createdMedia = _mediaService.CreateMedia(name, (int) memberMediaFolder, "");
            var convertedMedia = (PrivateLibraryImage) MediaFactory.Convert(createdMedia.Id, typeof(PrivateLibraryImage));

            convertedMedia.OriginalBlobId = originalFilename;
            convertedMedia.SmallThumbBlobId = smallThumbFilename;
            convertedMedia.LargeThumbBlobId = largeThumbFilename;

            convertedMedia.OriginalUrl = $"/Umbraco/Api/TempImage/GetImage?url={HttpUtility.UrlEncode(originalFilename)}";
            convertedMedia.SmallThumbUrl = $"/Umbraco/Api/TempImage/GetImage?url={HttpUtility.UrlEncode(originalFilename)}&size=small";
            convertedMedia.LargeThumbUrl = $"/Umbraco/Api/TempImage/GetImage?url={HttpUtility.UrlEncode(originalFilename)}&size=medium";

            createdMedia.SetValues(convertedMedia);

            _mediaService.Save(createdMedia, member.Id);

            return convertedMedia;
        }

        /// <summary>
        /// Delete an image 
        /// </summary>
        /// <param name="blobStoreId"></param>
        /// <returns></returns>
        public bool DeleteImage(string blobStoreId)
        {
            throw new NotImplementedException();
        }

        public bool RenameImage(string name, string newName)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IMedia> PopulateUsageCounts(IEnumerable<IMedia> mediaItems)
        {
            List<IMedia> result = new List<IMedia>();
            foreach (var mediaItem in mediaItems)
            {
                mediaItem.MailshotUses = _cmsImageService.GetImageUsageCount(mediaItem.MediaId);
                result.Add(mediaItem);
            }

            return result;
        }
    }
}