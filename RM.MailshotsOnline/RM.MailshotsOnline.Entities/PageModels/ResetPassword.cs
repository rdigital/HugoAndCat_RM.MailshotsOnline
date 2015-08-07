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
        #region Request form

        public string RequestIntroduction { get; set; }

        public string RequestEmailAddressLabel { get; set; }

        public string RequestPasswordResetCtaText { get; set; }

        public string BadTokenMessage { get; set; }

        #endregion


        #region Request complete

        public string RequestCompleteMessage { get; set; }

        public BasePage RequestCompleteContinuePage { get; set; }

        #endregion

        public RequestResetPasswordViewModel RequestViewModel { get; set; }

        
        #region Reset form

        public string ResetIntroduction { get; set; }

        public string ResetNewPasswordLabel { get; set; }

        public string ResetConfirmNewPasswordLabel { get; set; }

        public string ResetSavePasswordCtaText { get; set; }

        #endregion


        #region Reset complete

        public string ResetCompleteMessage { get; set; }

        public BasePage ResetCompleteContinuePage { get; set; }

        #endregion


        #region Emails

        public string RequestCompleteEmail { get; set; }

        #endregion

        public ResetPasswordViewModel ResetViewModel { get; set; }

    }
}
