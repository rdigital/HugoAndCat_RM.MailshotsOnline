using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using Microsoft.SqlServer.Server;
using RM.MailshotsOnline.Entities.ViewModels;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class RequestResetPassword : BasePage
    {
        #region Main form

        public string Introduction { get; set; }

        public string EmailAddressLabel { get; set; }

        public string PasswordResetCtaText { get; set; }

        #endregion

        #region Completed

        public string PasswordResetCompleteTitle { get; set; }

        public string PasswordResetCompleteMainText { get; set; }

        public BasePage ContinuePage { get; set; }

        #endregion

        public RequestResetPasswordViewModel ViewModel { get; set; }

    }
}
