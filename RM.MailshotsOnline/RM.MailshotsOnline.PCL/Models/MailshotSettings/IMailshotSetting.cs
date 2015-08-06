using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models.MailshotSettings
{
    public interface IMailshotSetting
    {
        string Name { get; set; }

        string XslData { get; set; }

        int JsonIndex { get; set; }

        int UmbracoPageId { get; set; }
    }
}
