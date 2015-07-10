using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMailshotContent
    {
        Guid MailshotContentId { get; set; }

        string Content { get; set; }
    }
}
