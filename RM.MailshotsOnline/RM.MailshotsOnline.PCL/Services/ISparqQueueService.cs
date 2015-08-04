using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ISparqQueueService
    {
        Task<bool> SendRenderJob(IMailshot mailshot);

        Task<bool> SendRenderAndPrintJob(IMailshot mailshot);
    }
}
