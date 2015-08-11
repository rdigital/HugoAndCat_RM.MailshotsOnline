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
        /// Get all images in the public library.
        /// </summary>
        /// <returns>The collection of images.</returns>
        public IEnumerable<LibraryImage> GetImages()
        {
            return Convert(_helper.TagQuery.GetMediaByTagGroup(ContentConstants.Settings.DefaultMediaLibraryTagGroup));
        }

        /// <summary>
        /// Gets all images under the given tag in the public library.
        /// </summary>
        /// <param name="tag">The tag</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<LibraryImage> GetImages(string tag)
        {
            return Convert(_helper.TagQuery.GetMediaByTag(tag, ContentConstants.Settings.DefaultMediaLibraryTagGroup));
        }

        /// <summary>
        /// Get all images belonging to the member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>The collection of images.</returns>
        public IEnumerable<LibraryImage> GetImages(IMember member)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<LibraryImage> Convert(IEnumerable<IPublishedContent> publishedContent)
        {
            if (publishedContent == null)
            {
                return null;
            }

            var concreteContent = publishedContent.Select(
                    x =>
                        new LibraryImage()
                        {
                            Tags = x.GetPropertyValue("tags").ToString().Split(','),
                            Src = x.GetPropertyValue("umbracoFile").ToString()
                        });

            return concreteContent;
        }
    }
}