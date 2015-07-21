using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.ViewModels;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class Register : BasePage
    {
        public string EmailLabel { get; set; }

        public string ConfirmEmailLabel { get; set; }

        public string TitleLabel { get; set; }

        public string FirstNameLabel { get; set; }

        public string LastNameLabel { get; set; }

        public string PasswordLabel { get; set; }

        public string ConfirmPasswordLabel { get; set; }

        public string PasswordReminderLabel { get; set; }

        public string RoyalMailContactOptionsDisclaimer { get; set; }

        public string RoyalMailContactOptionsLabel { get; set; }

        public string ThirdPartyContactOptionsDisclaimer { get; set; }

        public string ThirdPartyContactOptionsLabel { get; set; }

        public string AgreeToTermsAndConditionsLabel { get; set; }
    }
}
