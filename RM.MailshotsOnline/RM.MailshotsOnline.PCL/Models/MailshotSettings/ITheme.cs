using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models.MailshotSettings
{
    public interface ITheme : IMailshotSetting
    {
        Guid ThemeId { get; set; }
    }
}
