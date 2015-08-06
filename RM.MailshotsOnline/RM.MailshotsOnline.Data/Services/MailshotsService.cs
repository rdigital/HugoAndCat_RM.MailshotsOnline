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
        {
            _context = new StorageContext();
        }

        public IEnumerable<IMailshot> GetAllMailshots()
        {
            return _context.Mailshots.AsEnumerable();
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
                return _context.Mailshots.Where(m => m.UserId == userId);
            }
            else
            {
                return _context.Mailshots.Where(m => m.UserId == userId && m.Draft == true);
            }
        }

        public void SaveMailshot(IMailshot mailshot)
        {
            if (mailshot.MailshotId == Guid.Empty)
            {
                _context.Mailshots.Add((Mailshot)mailshot);
            }

            _context.SaveChanges();
        }

        public void Delete(IMailshot mailshot)
        {
            _context.Mailshots.Remove((Mailshot)mailshot);
            _context.SaveChanges();
        }
    }
}
