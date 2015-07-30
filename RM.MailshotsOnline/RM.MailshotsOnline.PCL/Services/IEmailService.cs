using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IEmailService
    {
        void SendMail(string from, string to, string subject, string body);
    }
}
