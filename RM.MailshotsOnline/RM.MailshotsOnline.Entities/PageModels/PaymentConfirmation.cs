using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class PaymentConfirmation : BasePage
    {
        #region Page content

        public string ConfirmationMessage { get; set; }

        public string ConfirmationEmailSubject { get; set; }

        public string ConfirmationEmailToUser { get; set; }

        #endregion

        #region Error messages

        public string PayPalErrorMessage { get; set; }

        public string CamapginErrorMessage { get; set; }

        #endregion

        public bool DisplayCampaignErrorMessage { get; set; }

        public bool DisplayPaypalErrorMessage { get; set; }
    }
}
