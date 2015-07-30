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
        public void SendMail(string @from, string to, string subject, string body)
        {
            var client = new SmtpClient();

            using (var message = new MailMessage(new MailAddress(from), new MailAddress(to))
            {
                Subject = subject,
                Body = body
            })
            {
                client.Send(message);
            };
        }
    }
}
