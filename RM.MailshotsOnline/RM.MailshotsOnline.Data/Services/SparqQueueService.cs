using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Services
{
    public class SparqQueueService : ISparqQueueService
    {
        private IMailshotsService _mailshotsService;

        public SparqQueueService(IMailshotsService mailshotsService)
        {
            _mailshotsService = mailshotsService;
        }

        public bool SendRenderAndPrintJob(IMailshot mailshot)
        {
            return SendJob(mailshot, true);
        }

        public bool SendRenderJob(IMailshot mailshot)
        {
            return SendJob(mailshot, false);
        }

        private bool SendJob(IMailshot mailshot, bool printAfterRender)
        {
            var success = true;
            //TODO: Generate XML based on Mailshot
            //TODO: Generate appropriate XSL based on Mailshot
            //TODO: Create new message for queue
            //TODO: Send to queue
            //TODO: Update status of Mailshot
            throw new NotImplementedException();
            return success;
        }
    }
}
