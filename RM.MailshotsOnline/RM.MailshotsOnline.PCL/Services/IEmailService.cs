using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Wrapper for sending a simple email
        /// </summary>
        /// <param name="from">Sender's address</param>
        /// <param name="to">Recipient's address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body text</param>
        void SendMail(string from, string to, string subject, string body);
    }
}
