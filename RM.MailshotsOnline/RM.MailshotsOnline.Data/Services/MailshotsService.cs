using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class MailshotsService : IMailshotsService
    {
        private StorageContext _context;

        public MailshotsService() 
            : this(new StorageContext())
        { }

        public MailshotsService(StorageContext storageContext)
        {
            _context = storageContext;
        }

        /// <summary>
        /// Finds all Mailshots
        /// </summary>
        /// <returns>Collection of Mailshots</returns>
        public IEnumerable<IMailshot> GetAllMailshots()
        {
            return _context.Mailshots.OrderBy(m => m.Name).AsEnumerable();
        }

        /// <summary>
        /// Checks if a mailshot belongs to a user
        /// </summary>
        /// <param name="mailshotId">ID of the mailshot</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>Returns true if the mailshot belongs to the user</returns>
        public bool MailshotBelongsToUser(Guid mailshotId, int userId)
        {
            return _context.Mailshots.Any(m => m.MailshotId == mailshotId && m.UserId == userId);
        }

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        public IMailshot GetMailshot(Guid mailshotId)
        {
            return _context.Mailshots
                .Include("Content")
                .Include("Template")
                .Include("Format")
                .Include("Theme")
                .FirstOrDefault(m => m.MailshotId == mailshotId);
        }

        /// <summary>
        /// Gets a user's mailshots
        /// </summary>
        /// <param name="userId">The ID of the user to search against</param>
        /// <param name="draftOnly">Indicates if only draft Mailshots should be returned</param>
        /// <returns>Collection of Mailshot objects</returns>
        public IEnumerable<IMailshot> GetUsersMailshots(int userId, bool draftOnly = false)
        {
            if (!draftOnly)
            {
                return _context.Mailshots.Where(m => m.UserId == userId).OrderBy(m => m.Name);
            }
            else
            {
                return _context.Mailshots.Where(m => m.UserId == userId && m.Draft == true).OrderBy(m => m.Name);
            }
        }

        /// <summary>
        /// Saves a Mailshot to the database
        /// </summary>
        /// <param name="mailshot"></param>
        /// <returns></returns>
        public IMailshot SaveMailshot(IMailshot mailshot)
        {
            if (mailshot.MailshotId == Guid.Empty)
            {
                _context.Mailshots.Add((Mailshot)mailshot);
            }

            _context.SaveChanges();
            return mailshot;
        }

        /// <summary>
        /// Deletes a Mailshot
        /// </summary>
        /// <param name="mailshot">The Mailshot to be deleted</param>
        /// <returns>True on success</returns>
        public bool Delete(IMailshot mailshot)
        {
            // Double check that the mailshot isn't used in a campaign
            if (_context.Campaigns.Any(c => c.MailshotId == mailshot.MailshotId))
            {
                return false;
            }

            // Unlink any images from the mailshot first
            var usedImages = _context.MailshotImageUse.Where(ui => ui.MailshotId == mailshot.MailshotId);
            _context.MailshotImageUse.RemoveRange(usedImages);

            // Remove the mailshot
            _context.Mailshots.Remove((Mailshot)mailshot);
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Updates the links between Mailshots and CMS images
        /// </summary>
        /// <param name="mailshot">Mailshot to link images to</param>
        /// <param name="linkedImages">List of image URLs to be linked</param>
        public void UpdateLinkedImages(IMailshot mailshot, IEnumerable<string> linkedImages)
        {
            if (mailshot.MailshotContentId != Guid.Empty)
            {
                // Unlink any existing from the mailshot first
                var usedImages = _context.MailshotImageUse.Where(ui => ui.MailshotId == mailshot.MailshotId);
                _context.MailshotImageUse.RemoveRange(usedImages);

                foreach (string src in linkedImages.Distinct())
                {
                    var cmsImage = _context.CmsImages.FirstOrDefault(c => c.Src == src);
                    if (cmsImage != null)
                    {
                        _context.MailshotImageUse.Add(new MailshotImageUse() { MailshotId = mailshot.MailshotId, CmsImageId = cmsImage.CmsImageId });
                    }
                }

                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Checks if the given mailshot is used in a campaign
        /// </summary>
        /// <param name="mailshot">Mailshot to check</param>
        /// <returns>True if the mailshot is used, false otherwise</returns>
        public bool MailshotIsUsedInCampaign(IMailshot mailshot)
        {
            return _context.Campaigns.Any(c => c.MailshotId == mailshot.MailshotId);
        }
    }
}
