using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class ModerationPage : BasePage
    {
        #region Content

        public string CampaignApprovedMessage { get; set; }

        public string CampaignRejectedMessage { get; set; }

        public string ConfirmButtonText { get; set; }

        public string PrintingCompleteMessage { get; set; }

        #endregion

        public bool DisplayApprovedMessage { get; set; }

        public bool DisplayRejectedMessage { get; set; }

        public bool DisplayConfirmPrintingMessage { get; set; }
    }
}
