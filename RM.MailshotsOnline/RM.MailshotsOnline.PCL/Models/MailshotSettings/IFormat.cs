using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models.MailshotSettings
{
    public interface IFormat : IMailshotSetting
    {
        Guid FormatId { get; set; }

        decimal PricePerPrint { get; set; }
    }
}
