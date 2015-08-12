using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glass.Mapper.Umb;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using IMember = RM.MailshotsOnline.PCL.Models.IMember;
using UmbracoContext = Umbraco.Web.UmbracoContext;

namespace RM.MailshotsOnline.Data.Services
{
    public class ImageLibraryService : IImageLibraryService
    {
        private readonly IUmbracoService _umbracoService;
        private readonly UmbracoHelper _helper = new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);

        public ImageLibraryService(IUmbracoService umbracoService)
        {
            _umbracoService = umbracoService;
        }

        /// <summary>
        /// Get a list of all tags stored in Umbraco
        /// </summary>
        /// <returns>The list of all tags</returns>
        public IEnumerable<string> GetTags()
        {
            return _helper.TagQuery.GetAllTags(ContentConstants.Settings.DefaultMediaLibraryTagGroup)
                .Select(x => x.Text);
        }

        /// <summary>
        /// Get all images in the public library.
        /// </summary>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IImage> GetImages()
        {
            return Convert(_helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup));
        }

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IImage> GetImages(string tag)
        {
            return Convert(_helper.TagQuery.GetMediaByTag(tag, ContentConstants.Settings.DefaultMediaLibraryTagGroup));
        }

        /// <summary>
        /// Get all images belonging to the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<IImage> GetImages(IMember member)
        {
            var privateImages =
                _helper.ContentQuery.TypedMedia(ContentConstants.MediaContent.PrivateMediaLibraryId)
                    .DescendantsOrSelf(ContentConstants.MediaContent.PrivateLibraryImageMediaTypeAlias);

            var memberPrivateImages =
                privateImages.Where(x => x.GetPropertyValue("username").Equals(member.Username.ToString()));

            return Convert(memberPrivateImages, true);
        }

        public bool AddImage(IImage image, IMember member)
        {
            throw new NotImplementedException();
        }

        public bool DeleteImage(IImage image)
        {
            throw new NotImplementedException();
        }

        public bool RenameImage(IImage image, string name)
        {
            throw new NotImplementedException();
        }


        private static IEnumerable<IImage> Convert(IEnumerable<IPublishedContent> publishedContent, bool @private = false)
        {
            if (publishedContent == null)
            {
                return null;
            }

            if (@private)
            {
                return publishedContent.Select(
                    x =>
                        new PrivateLibraryImage()
                        {
                            Src = x.GetPropertyValue("umbracoFile")?.ToString(),
                            Width = x.GetPropertyValue("umbracoWidth")?.ToString(),
                            Height = x.GetPropertyValue("umbracoHeight")?.ToString(),
                            Size = x.GetPropertyValue("umbracoBytes")?.ToString(),
                            Type = x.GetPropertyValue("umbracoExtension")?.ToString(),
                            Name = x.GetPropertyValue("umbracoName")?.ToString(),

                            Username = x.GetPropertyValue("username")?.ToString()
                        });
            }

            return publishedContent.Select(
                x =>
                    new PublicLibraryImage()
                    {
                        Src = x.GetPropertyValue("umbracoFile")?.ToString(),     
                        Width = x.GetPropertyValue("umbracoWidth")?.ToString(),
                        Height = x.GetPropertyValue("umbracoHeight")?.ToString(),
                        Size = x.GetPropertyValue("umbracoBytes")?.ToString(),
                        Type = x.GetPropertyValue("umbracoExtension")?.ToString(),
                        Name = x.GetPropertyValue("umbracoName")?.ToString(),

                        Tags = x.GetPropertyValue("tags")?.ToString().Split(',')
                    });
        }
    }
}