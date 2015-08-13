using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Data.Media_Conversion;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMember = RM.MailshotsOnline.PCL.Models.IMember;
using IMedia = RM.MailshotsOnline.PCL.Models.IMedia;
using UmbracoContext = Umbraco.Web.UmbracoContext;

namespace RM.MailshotsOnline.Data.Services
{
    public class ImageLibraryService : IImageLibraryService
    {
        private readonly IUmbracoService _umbracoService;
        private readonly ICmsImageService _cmsImageService;
        private readonly UmbracoHelper _helper = new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);

        public ImageLibraryService(IUmbracoService umbracoService, ICmsImageService cmsImageService)
        {
            _umbracoService = umbracoService;
            _cmsImageService = cmsImageService;
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
                PopulateUsageCounts(_helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup)
                    .Select(x => MediaFactory.Convert(x, typeof (PublicLibraryImage))));
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

        public bool AddImage(IMedia image, IMember member)
        {
            throw new NotImplementedException();
        }

        public bool DeleteImage(IMedia image)
        {
            throw new NotImplementedException();
        }

        public bool RenameImage(IMedia image, string name)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IMedia> PopulateUsageCounts(IEnumerable<IMedia> mediaItems)
        {
            List<IMedia> result = new List<IMedia>();
            foreach(var mediaItem in mediaItems)
            {
                mediaItem.MailshotUses = _cmsImageService.GetImageUsageCount(mediaItem.MediaId);
                result.Add(mediaItem);
            }

            return result;
        }
    }
}