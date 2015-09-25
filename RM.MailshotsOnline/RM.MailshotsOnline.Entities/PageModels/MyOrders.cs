using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    public class MyOrders : BasePage
    {
        #region Umbraco Properties

        public string ProcessingStatusText { get; set; }

        public string ProcessingStatusDescription { get; set; }

        public string DespatchedStatusText { get; set; }

        public string DespatchedStatusDescription { get; set; }

        public string CancelledStatusText { get; set; }

        public string CancelledStatusDescription { get; set; }

        public string FailedChecksStatusText { get; set; }

        public string FailedChecksStatusDescription { get; set; }

        public string PaymentFailedStatusText { get; set; }

        public string PaymentFailedStatusDescription { get; set; }

        public string DeliveredStatusText { get; set; }

        public string DeliveredStatusDescription { get; set; }

        public string OrderPlacedLabel { get; set; }

        public string OrderNumberLabel { get; set; }

        public string TotalLabel { get; set; }

        public string ActiveLabel { get; set; }

        public string NoOrdersMessage { get; set; }

        public int OrderDetailsPage { get; set; }

        #endregion

        public IEnumerable<ICampaign> Campaigns { get; set; }

        public ISettingsFromCms Settings { get; set; }

        public int ActiveCampaignCount
        {
            get
            {
                if (this.Campaigns == null)
                {
                    return 0;
                }
                else
                {
                    var activeStatuses = new List<PCL.Enums.CampaignStatus>()
                    {
                        PCL.Enums.CampaignStatus.Draft,
                        PCL.Enums.CampaignStatus.ReadyToCheckout,
                        PCL.Enums.CampaignStatus.PendingModeration,
                        PCL.Enums.CampaignStatus.ReadyForFulfilment,
                        PCL.Enums.CampaignStatus.SentForFulfilment
                    };

                    return Campaigns.Count(c => activeStatuses.Contains(c.Status));
                }
            }
        }
    }
}
