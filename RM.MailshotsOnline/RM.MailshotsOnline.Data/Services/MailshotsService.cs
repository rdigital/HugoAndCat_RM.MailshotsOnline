using RM.MailshotsOnline.Data.DAL;
using RM.MailshotsOnline.Data.Helpers;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class MailshotsService : IMailshotsService
    {
        private StorageContext _context;
        private HC.RM.Common.PCL.Persistence.IBlobService _blobService;
        private HC.RM.Common.PCL.Persistence.IBlobStorage _blobStorage;

        public MailshotsService() 
            : this(new StorageContext())
        { }

        public MailshotsService(StorageContext storageContext)
        {
            _context = storageContext;
            _blobStorage = new HC.RM.Common.Azure.Persistence.BlobStorage(ConfigHelper.PrivateStorageConnectionString);
            _blobService = new HC.RM.Common.Azure.Persistence.BlobService(_blobStorage, ConfigHelper.MailshotContentBlobContainer);
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
            var mailshot = _context.Mailshots
                .Include("Content")
                .Include("Template")
                .Include("Format")
                .Include("Theme")
                .FirstOrDefault(m => m.MailshotId == mailshotId);

            if (!string.IsNullOrEmpty(mailshot.ContentBlobId))
            {
                // Get blob content
                mailshot.ContentText = GetBlobContent(mailshot.ContentBlobId);
            }

            return mailshot;
        }

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        public async Task<IMailshot> GetMailshotAsync(Guid mailshotId)
        {
            var mailshot = _context.Mailshots
                .Include("Content")
                .Include("Template")
                .Include("Format")
                .Include("Theme")
                .FirstOrDefault(m => m.MailshotId == mailshotId);

            if (!string.IsNullOrEmpty(mailshot.ContentBlobId))
            {
                // Get blob content
                mailshot.ContentText = await GetBlobContentAsync(mailshot.ContentBlobId);
            }

            return mailshot;
        }

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        public IMailshot GetMailshotWithCampaignData(Guid mailshotId)
        {
            var mailshot = _context.Mailshots
                .Include("Content")
                .Include("Template")
                .Include("Format")
                .Include("Theme")
                .Include("Campaigns")
                .FirstOrDefault(m => m.MailshotId == mailshotId);

            if (!string.IsNullOrEmpty(mailshot.ContentBlobId))
            {
                // Get blob content
                mailshot.ContentText = GetBlobContent(mailshot.ContentBlobId);
            }

            return mailshot;
        }

        /// <summary>
        /// Gets a specific Mailshot
        /// </summary>
        /// <param name="mailshotId">The ID of the mailshot to get</param>
        /// <returns>Mailshot object</returns>
        public async Task<IMailshot> GetMailshotWithCampaignDataAsync(Guid mailshotId)
        {
            var mailshot = _context.Mailshots
                .Include("Content")
                .Include("Template")
                .Include("Format")
                .Include("Theme")
                .Include("Campaigns")
                .FirstOrDefault(m => m.MailshotId == mailshotId);

            if (!string.IsNullOrEmpty(mailshot.ContentBlobId))
            {
                // Get blob content
                mailshot.ContentText = await GetBlobContentAsync(mailshot.ContentBlobId);
            }

            return mailshot;
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
        public async Task<IMailshot> SaveMailshotAsync(IMailshot mailshot)
        {
            if (!string.IsNullOrEmpty(mailshot.ContentText))
            {
                // Save blob
                string blobId = mailshot.ContentBlobId;
                if (string.IsNullOrEmpty(blobId))
                {
                    blobId = string.Format("{0}/{1}.txt", mailshot.UserId, mailshot.MailshotId);
                }

                mailshot.ContentBlobId = await _blobService.StoreAsync(Encoding.UTF8.GetBytes(mailshot.ContentText), blobId, "text/plain");
            }

            return PerformSave(mailshot);
        }

        /// <summary>
        /// Saves a Mailshot to the database
        /// </summary>
        /// <param name="mailshot"></param>
        /// <returns></returns>
        public IMailshot SaveMailshot(IMailshot mailshot)
        {
            if (mailshot.MailshotId != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(mailshot.ContentText))
                {
                    // Save blob
                    string blobId = mailshot.ContentBlobId;
                    if (string.IsNullOrEmpty(blobId))
                    {
                        blobId = string.Format("{0}/{1}.txt", mailshot.UserId, mailshot.MailshotId);
                    }

                    mailshot.ContentBlobId = _blobService.Store(Encoding.UTF8.GetBytes(mailshot.ContentText), blobId, "text/plain");
                }
            }

            var savedMailshot = PerformSave(mailshot);

            if (mailshot.MailshotId == Guid.Empty && !string.IsNullOrEmpty(mailshot.ContentText))
            {
                // Save blob
                string blobId = mailshot.ContentBlobId;
                if (string.IsNullOrEmpty(blobId))
                {
                    blobId = string.Format("{0}/{1}.txt", mailshot.UserId, savedMailshot.MailshotId);
                }

                savedMailshot.ContentBlobId = _blobService.Store(Encoding.UTF8.GetBytes(mailshot.ContentText), blobId, "text/plain");
            }

            return PerformSave(savedMailshot);
        }

        private IMailshot PerformSave(IMailshot mailshot)
        {
            if (mailshot.MailshotId == Guid.Empty)
            {
                _context.Mailshots.Add((Mailshot)mailshot);
            }
            else
            {
                if (mailshot.MailshotContentId.HasValue)
                {
                    // Remove MailshotContent blocks for old mailshots
                    Guid mailshotContentId = Guid.Empty;
                    mailshotContentId = mailshot.MailshotContentId.Value;
                    mailshot.Content = null;
                    mailshot.MailshotContentId = null;
                    var mailshotContent = _context.MailshotContents.FirstOrDefault(mc => mc.MailshotContentId == mailshotContentId);
                    if (mailshotContentId != null)
                    {
                        _context.MailshotContents.Remove(mailshotContent);
                    }
                }
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

            // Remove the mailshot content
            var contentEntity = _context.MailshotContents.FirstOrDefault(mc => mc.MailshotContentId == mailshot.MailshotContentId);

            // Remove the blob
            if (!string.IsNullOrEmpty(mailshot.ContentBlobId))
            {
                _blobService.DeleteBlob(mailshot.ContentBlobId);
            }

            // Remove the mailshot
            _context.Mailshots.Remove((Mailshot)mailshot);
            _context.MailshotContents.Remove(contentEntity);
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

        /// <summary>
        /// Fetches the content of a given blob
        /// </summary>
        /// <param name="blobId">Path of the blob</param>
        /// <returns></returns>
        private async Task<string> GetBlobContentAsync(string blobId)
        {
            using (MemoryStream blobStream = await _blobService.DownloadToStreamAsync(blobId) as MemoryStream)
            {
                if (blobStream != null)
                {
                    var bytes = blobStream.ToArray();
                    var output = Encoding.UTF8.GetString(bytes);
                    return output;
                }

                return null;
            }
        }

        /// <summary>
        /// Fetches the content of a given blob
        /// </summary>
        /// <param name="blobId">Path of the blob</param>
        /// <returns></returns>
        private string GetBlobContent(string blobId)
        {
            using (MemoryStream blobStream = _blobService.DownloadToStream(blobId) as MemoryStream)
            {
                if (blobStream != null)
                {
                    var bytes = blobStream.ToArray();
                    var output = Encoding.UTF8.GetString(bytes);
                    return output;
                }

                return null;
            }
        }
    }
}
