using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IMailshotsService
    {
        IEnumerable<IMailshot> GetAllMailshots();

        IEnumerable<IMailshot> GetUsersMailshots(int userId, bool draftOnly = false);

        IMailshot GetMailshot(Guid mailshotId);

        IMailshot SaveMailshot(IMailshot mailshot);

        void Delete(IMailshot mailshot);

        void UpdateLinkedImages(IMailshot mailshot, IEnumerable<string> linkedImages);

        /// <summary>
        /// Checks if the given mailshot is used in a campaign
        /// </summary>
        /// <param name="mailshot">Mailshot to check</param>
        /// <returns>True if the mailshot is used, false otherwise</returns>
        bool MailshotIsUsedInCampaign(IMailshot mailshot);
    }
}
