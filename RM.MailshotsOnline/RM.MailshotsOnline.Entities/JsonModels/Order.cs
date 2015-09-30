using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class Order
    {
        public Guid CampaignId { get; set; }

        public string CampaignTitle { get; set; }

        public string OrderNumber { get; set; }

        public string OrderDetailsUrl { get; set; }

        public bool ShowCancelLink { get; set; }

        public string InvoiceUrl { get; set; }

        public string Status { get; set; }

        public string StatusText { get; set; }

        public string StatusDescription { get; set; }

        public DateTime OrderPlaced { get; set; }

        public Decimal Total { get; set; }
    }
}
