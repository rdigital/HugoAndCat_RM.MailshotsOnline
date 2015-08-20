using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Data.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        /// <summary>
        /// Wrapper for sending a simple email
        /// </summary>
        /// <param name="from">Sender's address</param>
        /// <param name="to">Recipient's address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body text</param>
        public void SendMail(string @from, string to, string subject, string body)
        {
            var client = new SmtpClient();

            using (var message = new MailMessage(new MailAddress(from), new MailAddress(to))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                client.Send(message);
            };
        }
    }
}
