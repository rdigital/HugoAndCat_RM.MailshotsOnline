using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models.MailshotSettings
{
    public interface IMailshotDefaultContent : IMailshotSetting
    {
        Guid MailshotDefaultContentId { get; set; }
    }
}
