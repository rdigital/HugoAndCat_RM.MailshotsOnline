using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class OrderDetails : BasePage
    {
        public string CostBreakdownLabel { get; set; }

        public string BillingAddressLabel { get; set; }

        public MyOrders MyOrdersPage { get; set; }

        public string PrintAndDeliveryHeading { get; set; }

        public string ShouldArriveByText { get; set; }

        public string ServiceFeesHeading { get; set; }

        public string MailshotsOnlineServiceFeeHeading { get; set; }

        public string DataSearchFeeHeading { get; set; }

        public string PostageLabel { get; set; }

        public string TaxHeading { get; set; }

        public string VatLabel { get; set; }

        public string TotalCostLabel { get; set; }

        public string OurDataHeading { get; set; }

        public string YourDataHeading { get; set; }

        public string RecipientsHeading { get; set; }

        public bool DisplayCampaignErrorMessage { get; set; }

        public ICampaign Campaign { get; set; }

        public Dictionary<string, List<IInvoiceLineItem>> CostBreakdown { get; set; }
    }
}
