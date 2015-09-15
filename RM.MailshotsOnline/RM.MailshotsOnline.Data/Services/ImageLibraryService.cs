using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
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
using Examine;

namespace RM.MailshotsOnline.Data.Services
{
    public class ImageLibraryService : IImageLibraryService
    {
        private readonly UmbracoHelper _helper = new UmbracoHelper(UmbracoContext.Current);
        private readonly BlobStorageHelper _blobStorage =
            new BlobStorageHelper(ConfigHelper.PrivateStorageConnectionString, ConfigHelper.PrivateMediaBlobStorageContainer);
        private readonly ILogger _logger;
        private readonly ICmsImageService _cmsImageService;
        private readonly IMediaService _mediaService;
        private readonly ImageResizer _imageResizer = new ImageResizer();

        private object searchLock = new object();

        public ImageLibraryService(ILogger logger, ICmsImageService cmsImageService)

        {
            _logger = logger;
            _cmsImageService = cmsImageService;
            _mediaService = UmbracoContext.Current.Application.Services.MediaService;
        }

        /// <summary>
        /// Get a list of all tags stored in Umbraco.
        /// </summary>
        /// <returns>The list of all tags.</returns>
        public IEnumerable<string> GetTags()
        {
            var tags = UmbracoContext.Current.Application.Services.TagService.GetAllTags(ContentConstants.Settings.DefaultMediaLibraryTagGroup);
            return tags.Select(t => t.Text);
        }

        /// <summary>
        /// Get all images in the public library.
        /// </summary>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages()
        {
            
            var images = UmbracoContext.Current.Application.Services.TagService.GetTaggedMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup).Select(te => _helper.TypedMedia(te.EntityId));
            var publicImages = images.Select(x => MediaFactory.Convert(x, typeof(PublicLibraryImage)));
            return PopulateUsageCounts(publicImages);
            //return PopulateUsageCounts(_helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup).Select(x => MediaFactory.Convert(x, typeof(PublicLibraryImage))));
        }

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages(string tag)
        {
            
            var images = UmbracoContext.Current.Application.Services.TagService.GetTaggedMediaByTag(tag, ContentConstants.Settings.DefaultMediaLibraryTagGroup).Select(te => _helper.TypedMedia(te.EntityId));
            var publicImages = images.Select(x => MediaFactory.Convert(x, typeof(PublicLibraryImage)));
            return PopulateUsageCounts(publicImages);
        }

        /// <summary>
        /// Gets an image
        /// </summary>
        /// <param name="mediaId">The Umbraco media ID of the image to fetch</param>
        /// <param name="publicImage">Indicates if this is a public image</param>
        /// <returns>The Media item</returns>
        public IMedia GetImage(int mediaId, bool publicImage)
        {
            var requiredType = publicImage ? typeof(PublicLibraryImage) : typeof(PrivateLibraryImage);
            IMedia image = MediaFactory.Convert(_helper.TypedMedia(mediaId), requiredType);
            image.MailshotUses = _cmsImageService.GetImageUsageCount(mediaId);
            return image;
        }

        /// <summary>
        /// Gets a media item (base class)
        /// </summary>
        /// <param name="mediaId">The Umbraco media ID of the media item to fetch</param>
        /// <returns>The Media item</returns>
        public IMedia GetMedia(int mediaId)
        {
            IMedia image = MediaFactory.Convert(_helper.TypedMedia(mediaId), typeof(Media));
            image.MailshotUses = _cmsImageService.GetImageUsageCount(mediaId);
            return image;
        }

        /// <summary>
        /// Finds a private image based on its Blob URL
        /// </summary>
        /// <param name="blobUrl">The Blob URL to search for</param>
        /// <returns>The Media item</returns>
        public IMedia GetImageByBlobUrl(string blobUrl)
        {
            lock(searchLock)
            {
            var mediaSearcher = ExamineManager.Instance.SearchProviderCollection["MediaSearcher"] ?? ExamineManager.Instance.DefaultSearchProvider;
            if (mediaSearcher != null)
            {
                var searchResults = mediaSearcher.Search(blobUrl, false);
                if (searchResults != null)
                {
                    if (searchResults.TotalItemCount > 0)
                    {
                            _logger.Info(this.GetType().Name, "GetImageByBlobUrl", "{0} search results for blob URL {1}", searchResults.Count(), blobUrl);
                        foreach (var searchResult in searchResults)
                        {
                                _logger.Info(this.GetType().Name, "GetImageByBlobUrl", "Found image with Media ID {0} for URL {1}", searchResult.Id, blobUrl);
                            var mediaId = searchResult.Id;
                            return GetImage(mediaId, false);
                        }
                    }
                    else
                    {
                        _logger.Info(this.GetType().Name, "GetImageByBlobUrl", "Search for blob ID {0} returned no results.", blobUrl);
                    }
                }
                else
                {
                    _logger.Error(this.GetType().Name, "GetImageByBlobUrl", "Examine searcher returned null result object.");
                }
            }
            else
            {
                _logger.Error(this.GetType().Name, "GetImageByBlobUrl", "Unable to get examine media searcher.");
            }

            return null;
        }
        }

        /// <summary>
        /// Get all images belonging to the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IMedia> GetImages(IMember member)
        {
            var privateMediaFolder = _helper.ContentQuery.TypedMedia(Constants.Constants.MediaContent.PrivateMediaLibraryId);
            var memberMediaFolder = privateMediaFolder.Children.FirstOrDefault(x => x.Name.Equals(member.Username));
            if (memberMediaFolder != null)
            {
                var privateImages = memberMediaFolder.Children().Where(x => x.ContentType.Alias == Constants.Constants.MediaContent.PrivateLibraryImageMediaTypeAlias);
                return PopulateUsageCounts(privateImages.Select(x => MediaFactory.Convert(x, typeof(PrivateLibraryImage))));
            }
            else
            {
                var results = new List<PrivateLibraryImage>();
                return results;
            }
        }


        /// <summary>
        /// Add an image into the blob store for the given member.
        /// </summary>
        /// <param name="bytes">The bytes representing the image.</param>
        /// <param name="name">The name of the image.</param>
        /// <param name="member">The member creating this image. The image will be stored in this member's personal image store.</param>
        /// <returns></returns>
        public IMedia AddImage(byte[] bytes, string name, IMember member)
        {
            byte[] smallThumb;
            byte[] largeThumb;
            System.Drawing.Image original;
            int originalHeight = 0;
            int originalWidth = 0;
            string extension;
            try
            {
                using (var stream = new MemoryStream(bytes))
                {
                    original = System.Drawing.Image.FromStream(stream);
                    smallThumb = _imageResizer.ResizeImageBytes(original, Constants.Constants.Settings.ImageThumbnailSizeSmall);
                    largeThumb = _imageResizer.ResizeImageBytes(original, Constants.Constants.Settings.ImageThumbnailSizeLarge);

                    extension = original.RawFormat.ToFileExtension();
                    originalHeight = original.Height;
                    originalWidth = original.Width;
                }
                //original = _imageResizer.GetImage(bytes);
                //smallThumb = _imageResizer.ResizeImageBytes(original, ContentConstants.Settings.ImageThumbnailSizeSmall);
                //largeThumb = _imageResizer.ResizeImageBytes(original, ContentConstants.Settings.ImageThumbnailSizeLarge);
            }
            catch (Exception e)
            {
                _logger.Error(this.GetType().Name, "AddImage", $"{e.GetType().Name} {e.Message} {e.StackTrace}");

                return null;
            }

            //var extension = original.RawFormat.GetType().Name.ToLower();
            var originalFilename = $"{member.Id}/{name}-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{extension.ToLower()}";
            var smallThumbFilename = $"{member.Id}/{name}-thumbSmall-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{extension.ToLower()}";
            var largeThumbFilename = $"{member.Id}/{name}-thumbLarge-{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{extension.ToLower()}";

            try
            {
                _blobStorage.StoreBytes(bytes, originalFilename, $"image/{extension.ToLower().Trim(".")}");
                _blobStorage.StoreBytes(smallThumb, smallThumbFilename, $"image/{extension.ToLower().Trim(".")}");
                _blobStorage.StoreBytes(largeThumb, largeThumbFilename, $"image/{extension.ToLower().Trim(".")}");
            }
            catch (Exception e)
            {
                _logger.Error(this.GetType().Name, "AddImage", $"{e.GetType().Name} {e.Message} {e.StackTrace}");

                return null;
            }


            var privateMediaFolder = _helper.ContentQuery.TypedMedia(Constants.Constants.MediaContent.PrivateMediaLibraryId);
            var memberMediaFolder = privateMediaFolder.Children.FirstOrDefault(x => x.Name.Equals(member.Username));
            int? memberMediaFolderId;
            if (memberMediaFolder == null)
            {
                var createdFolder = _mediaService.CreateMedia(member.Username, Constants.Constants.MediaContent.PrivateMediaLibraryId,
                    Constants.Constants.MediaContent.PrivateImageLibraryFolderMediaTypeAlias);
                _mediaService.Save(createdFolder);
                memberMediaFolderId = createdFolder?.Id;
            }
            else
            {
                memberMediaFolderId = memberMediaFolder.Id;
            }


            var createdMedia = _mediaService.CreateMedia(name, (int)memberMediaFolderId, Constants.Constants.MediaContent.PrivateLibraryImageMediaTypeAlias);
            var convertedMedia = (PrivateLibraryImage) MediaFactory.Convert(createdMedia, typeof(PrivateLibraryImage));

            convertedMedia.OriginalBlobId = originalFilename;
            convertedMedia.SmallThumbBlobId = smallThumbFilename;
            convertedMedia.LargeThumbBlobId = largeThumbFilename;

            convertedMedia.OriginalUrl = $"/Umbraco/Api/ImageLibrary/GetPrivateImage?url={HttpUtility.UrlEncode(originalFilename)}";
            convertedMedia.SmallThumbUrl = $"/Umbraco/Api/ImageLibrary/GetPrivateImage?url={HttpUtility.UrlEncode(originalFilename)}&size=small";
            convertedMedia.LargeThumbUrl = $"/Umbraco/Api/ImageLibrary/GetPrivateImage?url={HttpUtility.UrlEncode(originalFilename)}&size=medium";

            convertedMedia.Username = member.Username;
            convertedMedia.Height = originalHeight.ToString();
            convertedMedia.Width = originalWidth.ToString();
            convertedMedia.Type = extension.Trim(".");

            createdMedia.SetValues(convertedMedia);

            _mediaService.Save(createdMedia);

            return convertedMedia;
        }

        /// <summary>
        /// Delete an image 
        /// </summary>
        /// <param name="blobStoreId"></param>
        /// <returns></returns>
        public bool DeleteImage(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a media item
        /// </summary>
        /// <param name="media">The media item to be deleted</param>
        /// <returns>True on success</returns>
        public bool DeleteImage(IMedia media)
        {
            bool success = true;
            var umbracoMedia = _mediaService.GetById(media.MediaId);
            try
            {
                _mediaService.Delete(umbracoMedia);
            }
            catch (Exception ex)
            {
                _logger.Exception(this.GetType().Name, "DeleteImage", ex);
                _logger.Error(this.GetType().Name, "DeleteImage", "Unable to delete media item with ID {0}.", media.MediaId);

                if (ex.Source == "Examine")
                {
                    // TODO: Trigger rebuild of index
                    ExamineManager.Instance.RebuildIndex();
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Renames a given image (Not sure if this needs to be used?)
        /// </summary>
        /// <param name="name">Original name of the image</param>
        /// <param name="newName">New name for the image</param>
        /// <returns>True on success</returns>
        public bool RenameImage(string name, string newName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the usage count for a given set of media items and populates it onto the model
        /// </summary>
        /// <param name="mediaItems">Collection of media items</param>
        /// <returns>Collection of media items</returns>
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