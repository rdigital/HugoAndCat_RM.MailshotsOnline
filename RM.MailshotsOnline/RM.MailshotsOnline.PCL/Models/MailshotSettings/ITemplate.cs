using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models.MailshotSettings
{
    public interface ITemplate : IMailshotSetting
    {
        Guid TemplateId { get; set; }

        string XslData { get; set; }

        int FormatUmbracoPageId { get; set; }
    }
}
