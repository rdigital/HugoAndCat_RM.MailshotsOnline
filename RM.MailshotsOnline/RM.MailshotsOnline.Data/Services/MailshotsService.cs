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

        public IEnumerable<IMailshot> GetAllMailshots()
        {
            return _context.Mailshots.OrderBy(m => m.Name).AsEnumerable();
        }

        public IMailshot GetMailshot(Guid mailshotId)
        {
            return _context.Mailshots
                .Include("Content")
                .Include("Template")
                .Include("Format")
                .Include("Theme")
                .FirstOrDefault(m => m.MailshotId == mailshotId);
        }

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

        public IMailshot SaveMailshot(IMailshot mailshot)
        {
            if (mailshot.MailshotId == Guid.Empty)
            {
                _context.Mailshots.Add((Mailshot)mailshot);
            }

            _context.SaveChanges();
            return mailshot;
        }

        public void Delete(IMailshot mailshot)
        {
            // Unlink any images from the mailshot first
            var usedImages = _context.MailshotImageUse.Where(ui => ui.MailshotId == mailshot.MailshotId);
            _context.MailshotImageUse.RemoveRange(usedImages);

            // Remove the mailshot
            _context.Mailshots.Remove((Mailshot)mailshot);
            _context.SaveChanges();
        }

        public void UpdateLinkedImages(IMailshot mailshot, IEnumerable<string> linkedImages)
        {
            if (mailshot.MailshotContentId != Guid.Empty)
            {
                // Unlink any existing from the mailshot first
                var usedImages = _context.MailshotImageUse.Where(ui => ui.MailshotId == mailshot.MailshotId);
                _context.MailshotImageUse.RemoveRange(usedImages);

                foreach (string src in linkedImages)
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
    }
}
