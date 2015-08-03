using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.Business.Processors;

namespace SparqTemp
{
    public class SparqQueueService : ISparqQueueService
    {
        public bool SendRenderAndPrintJob(IMailshot mailshot)
        {
            throw new NotImplementedException();
        }

        public bool SendRenderJob(IMailshot mailshot)
        {
            var success = false;
            var mailshotProcessor = new MailshotsProcessor();
            var content = mailshotProcessor.GetXmlAndXslForMailshot(mailshot);

            // TODO: Send render job content
            
            return success;
        }
    }
}
