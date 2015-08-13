using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Entities.DataModels;

namespace RM.MailshotsOnline.Data.Services
{
    public class CmsImageService : ICmsImageService
    {
        private StorageContext _context;

        public CmsImageService()
            : this(new StorageContext())
        { }

        public CmsImageService(StorageContext storageContext)
        {
            _context = storageContext;
        }

        /// <summary>
        /// Delete a CMS image
        /// </summary>
        /// <param name="imageSrc">The URL of the CMS image to delete</param>
        /// <returns>True if the delete was successful.  False if the image is linked to a mailshot and can't be deleted</returns>
        public bool DeleteCmsImage(string imageSrc)
        {
            var cmsImage = GetCmsImage(imageSrc);
            return DeleteCmsImage(cmsImage);
        }

        /// <summary>
        /// Delete a CMS image
        /// </summary>
        /// <param name="umbracoImageId">The Umbraco ID of the CMS image to delete</param>
        /// <returns>True if the delete was successful.  False if the image is linked to a mailshot and can't be deleted</returns>
        public bool DeleteCmsImage(int umbracoImageId)
        {
            var cmsImage = GetCmsImage(umbracoImageId);
            return DeleteCmsImage(cmsImage);
        }

        /// <summary>
        /// Delete a CMS image
        /// </summary>
        /// <param name="cmsImage">The CMS image to delete</param>
        /// <returns>True if the delete was successful.  False if the image is linked to a mailshot and can't be deleted</returns>
        public bool DeleteCmsImage(ICmsImage cmsImage)
        {
            if (cmsImage != null)
            {
                if (!IsImageUsedInMailshot(cmsImage))
                {
                    _context.CmsImages.Remove((CmsImage)cmsImage);
                    _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a CMS image
        /// </summary>
        /// <param name="imageSrc">The URL of the CMS image</param>
        /// <returns>The CMS image object</returns>
        public ICmsImage GetCmsImage(string imageSrc)
        {
            return _context.CmsImages.Include("MailshotUses").FirstOrDefault(i => i.Src == imageSrc);
        }

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="umbracoImageId">The Umbraco ID of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        public ICmsImage GetCmsImage(int umbracoImageId)
        {
            return _context.CmsImages.Include("MailshotUses").FirstOrDefault(i => i.UmbracoMediaId == umbracoImageId);
        }

        /// <summary>
        /// Gets a CMS image
        /// </summary>
        /// <param name="cmsImageId">The ID of the CMS image</param>
        /// <returns>The CMS image object</returns>
        public ICmsImage GetCmsImage(Guid cmsImageId)
        {
            return _context.CmsImages.Include("MailshotUses").FirstOrDefault(i => i.CmsImageId == cmsImageId);
        }

        /// <summary>
        /// Gets the number of times a CMS image has been used in mailshots
        /// </summary>
        /// <param name="umbracoImageId">Umbraco ID of the CMS image</param>
        /// <returns>Number of times the image has been linked to a Mailshot</returns>
        public int GetImageUsageCount(int umbracoImageId)
        {
            var linkIds = from m in _context.MailshotImageUse
                          join c in _context.CmsImages on m.CmsImageId equals c.CmsImageId
                          where c.UmbracoMediaId == umbracoImageId
                          select m.MailshotImageUseId;

            return linkIds.Count(); 
        }

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="imageSrc">The URL of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        public bool IsImageUsedInMailshot(string imageSrc)
        {
            var image = GetCmsImage(imageSrc);
            return IsImageUsedInMailshot(image);
        }

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="umbracoImageId">The Umbraco ID of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        public bool IsImageUsedInMailshot(int umbracoImageId)
        {
            var image = GetCmsImage(umbracoImageId);
            return IsImageUsedInMailshot(image);
        }

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="cmsImageId">The ID of the CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        public bool IsImageUsedInMailshot(Guid cmsImageId)
        {
            var image = GetCmsImage(cmsImageId);
            return IsImageUsedInMailshot(image);
        }

        /// <summary>
        /// Checks if a CMS image is used in any mailshots
        /// </summary>
        /// <param name="cmsImage">The CMS image to check</param>
        /// <returns>True if the image is used, false if it is not used or not found</returns>
        public bool IsImageUsedInMailshot(ICmsImage cmsImage)
        {
            if (cmsImage == null)
            {
                return false;
            }

            return _context.MailshotImageUse.Any(u => u.CmsImageId == cmsImage.CmsImageId);
        }

        /// <summary>
        /// Links a CMS image to a mailshot
        /// </summary>
        /// <param name="cmsImage">The CMS image</param>
        /// <param name="mailshot">The Mailshot</param>
        public void LinkImageToMailshot(ICmsImage cmsImage, IMailshot mailshot)
        {
            if (!_context.MailshotImageUse.Any(iu => iu.CmsImageId == cmsImage.CmsImageId && iu.MailshotId == mailshot.MailshotId))
            {
                var imageUse = new MailshotImageUse()
                {
                    MailshotId = mailshot.MailshotId,
                    CmsImageId = cmsImage.CmsImageId
                };

                _context.MailshotImageUse.Add(imageUse);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Saves a CMS image to the database
        /// </summary>
        /// <param name="cmsImage">CMS image to save</param>
        /// <returns>The saved CMS image</returns>
        public ICmsImage SaveCmsImage(ICmsImage cmsImage)
        {
            if (cmsImage.CmsImageId == Guid.Empty)
            {
                _context.CmsImages.Add((CmsImage)cmsImage);
            }

            _context.SaveChanges();
            return cmsImage;
        }

        /// <summary>
        /// Unlinks a CMS image from a mailshot
        /// </summary>
        /// <param name="cmsImage">The CMS image</param>
        /// <param name="mailshot">The Mailshot</param>
        public void UnlinkImageFromMailshot(ICmsImage cmsImage, IMailshot mailshot)
        {
            var link = _context.MailshotImageUse.FirstOrDefault(iu => iu.CmsImageId == cmsImage.CmsImageId && iu.MailshotId == mailshot.MailshotId);
            if (link != null)
            {
                _context.MailshotImageUse.Remove((MailshotImageUse)link);
                _context.SaveChanges();
            }
        }
    }
}
