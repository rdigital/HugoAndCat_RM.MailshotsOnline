using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class PaymentCancelled : BasePage
    {
        #region Umbraco content

        public string Content { get; set; }

        #endregion


        public bool DisplayCampaignErrorMessage { get; set; }

        public bool DisplayPaypalErrorMessage { get; set; }
    }
}
