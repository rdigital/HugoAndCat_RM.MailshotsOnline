using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IMailshotSettingsService
    {
        void AddOrUpdateFormat(IFormat format);

        void AddOrUpdateTemplate(ITemplate template);

        void AddOrUpdateTheme(ITheme theme);

        IFormat GetFormatByJsonIndex(int index);

        ITemplate GetTemplateByJsonIndex(int index, int formatIndex);

        ITheme GetThemeByJsonIndex(int index);
    }
}
