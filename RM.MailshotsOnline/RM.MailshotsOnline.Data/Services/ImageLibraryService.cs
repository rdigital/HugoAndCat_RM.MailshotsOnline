using System;
using System.Collections.Generic;
using System.Linq;
using RM.MailshotsOnline.Data.Constants;
using RM.MailshotsOnline.Entities.JsonModels;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core.Models;
using Umbraco.Web;
using IMember = RM.MailshotsOnline.PCL.Models.IMember;

namespace RM.MailshotsOnline.Data.Services
{
    public class ImageLibraryService : IImageLibraryService
    {
        private readonly UmbracoHelper _helper = new Umbraco.Web.UmbracoHelper(UmbracoContext.Current);

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
        public IEnumerable<Image> GetImages()
        {
            return Convert(_helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup));
        }

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<Image> GetImages(string tag)
        {
            return Convert(_helper.TagQuery.GetMediaByTag(tag, ContentConstants.Settings.DefaultMediaLibraryTagGroup));
        }

        /// <summary>
        /// Get all images belonging to the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<Image> GetImages(IMember member)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Image> Convert(IEnumerable<IPublishedContent> publishedContent, bool @private = false)
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
                            Src = x.GetPropertyValue("umbracoFile").ToString(),
                            Width = x.GetPropertyValue("umbracoWidth").ToString(),
                            Height = x.GetPropertyValue("umbracoHeight").ToString(),
                            Size = x.GetPropertyValue("umbracoBytes").ToString(),
                            Type = x.GetPropertyValue("umbracoExtension").ToString(),

                            Username = x.GetProperty("username").ToString()
                        });
            }

            return publishedContent.Select(
                x =>
                    new PublicLibraryImage()
                    {
                        Src = x.GetPropertyValue("umbracoFile").ToString(),
                        Width = x.GetPropertyValue("umbracoWidth").ToString(),
                        Height = x.GetPropertyValue("umbracoHeight").ToString(),
                        Size = x.GetPropertyValue("umbracoBytes").ToString(),
                        Type = x.GetPropertyValue("umbracoExtension").ToString(),

                        Tags = x.GetPropertyValue("tags").ToString().Split(',')
                    });
        }
    }
}