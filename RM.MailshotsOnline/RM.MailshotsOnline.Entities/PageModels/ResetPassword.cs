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
    public class ResetPassword : BasePage
    {
        #region Main form

        public string Introduction { get; set; }

        public string NewPasswordLabel { get; set; }

        public string ConfirmNewPasswordLabel { get; set; }

        #endregion

        #region Completed

        public string CompletedMessage { get; set; }

        public BasePage ContinuePage { get; set; }

        #endregion

        public ResetPasswordViewModel ViewModel { get; set; }

    }
}
