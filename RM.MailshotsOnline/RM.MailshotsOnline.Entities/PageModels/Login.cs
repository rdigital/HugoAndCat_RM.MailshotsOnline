using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class Login : BasePage
    {
        public string EmailLabel { get; set; }

        public string PasswordLabel { get; set; }

        public string LoginCtaText { get; set; }

        public BasePage PasswordResetPage { get; set; }

        public string PasswordResetCtaText { get; set; }

        public string BadLoginMessage { get; set; }

        public string RegisterPrompt { get; set; }

        public string RegisterCtaText { get; set; }

        public BasePage RegisterPage { get; set; }

    }
}
