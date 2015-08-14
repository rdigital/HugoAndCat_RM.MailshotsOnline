using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ICmsImageService
    {
        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="cmsImage">The CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        bool IsImageUsedInMailshot(ICmsImage cmsImage);

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="cmsImageId">The ID of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        bool IsImageUsedInMailshot(Guid cmsImageId);

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="imageSrc">The URL of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        bool IsImageUsedInMailshot(string imageSrc);

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="umbracoImageId">The Umbraco ID of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        bool IsImageUsedInMailshot(int umbracoImageId);

        /// <summary>
        /// Gets a CMS image
        /// </summary>
        /// <param name="cmsImageId">The ID of the CMS image</param>
        /// <returns>The CMS image object</returns>
        ICmsImage GetCmsImage(Guid cmsImageId);

        /// <summary>
        /// Gets a CMS image
        /// </summary>
        /// <param name="imageSrc">The URL of the CMS image</param>
        /// <returns>The CMS image object</returns>
        ICmsImage GetCmsImage(string imageSrc);

        /// <summary>
        /// Gets a CMS image
        /// </summary>
        /// <param name="umbracoImageId">The Umbraco ID of the CMS image</param>
        /// <returns>The CMS image object</returns>
        ICmsImage GetCmsImage(int umbracoImageId);

        /// <summary>
        /// Gets the number of times a CMS image has been used in mailshots
        /// </summary>
        /// <param name="umbracoImageId">Umbraco ID of the CMS image</param>
        /// <returns>Number of times the image has been linked to a Mailshot</returns>
        int GetImageUsageCount(int umbracoImageId);

        /// <summary>
        /// Delete a CMS image
        /// </summary>
        /// <param name="cmsImage">The CMS image to delete</param>
        /// <returns>True if the delete was successful.  False if the image is linked to a mailshot and can't be deleted</returns>
        bool DeleteCmsImage(ICmsImage cmsImage);

        /// <summary>
        /// Delete a CMS image
        /// </summary>
        /// <param name="umbracoImageId">The Umbraco ID of the CMS image to delete</param>
        /// <returns>True if the delete was successful.  False if the image is linked to a mailshot and can't be deleted</returns>
        bool DeleteCmsImage(int umbracoImageId);

        /// <summary>
        /// Delete a CMS image
        /// </summary>
        /// <param name="imageSrc">The URL of the CMS image to delete</param>
        /// <returns>True if the delete was successful.  False if the image is linked to a mailshot and can't be deleted</returns>
        bool DeleteCmsImage(string imageSrc);

        /// <summary>
        /// Links a CMS image to a mailshot
        /// </summary>
        /// <param name="cmsImage">The CMS image</param>
        /// <param name="mailshot">The Mailshot</param>
        void LinkImageToMailshot(ICmsImage cmsImage, IMailshot mailshot);

        /// <summary>
        /// Saves a CMS image to the database
        /// </summary>
        /// <param name="cmsImage">CMS image to save</param>
        /// <returns>The saved CMS image</returns>
        ICmsImage SaveCmsImage(ICmsImage cmsImage);

        /// <summary>
        /// Unlinks a CMS image from a mailshot
        /// </summary>
        /// <param name="cmsImage">The CMS image</param>
        /// <param name="mailshot">The Mailshot</param>
        void UnlinkImageFromMailshot(ICmsImage cmsImage, IMailshot mailshot);

        /// <summary>
        /// Finds the CMS images that a user has used
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>Collection of images</returns>
        IEnumerable<ICmsImage> FindImagesUsedByUser(int userId);
    }
}
