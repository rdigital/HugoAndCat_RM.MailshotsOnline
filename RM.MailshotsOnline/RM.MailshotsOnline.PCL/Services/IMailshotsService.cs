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

        void SaveMailshot(IMailshot mailshot);
    }
}
