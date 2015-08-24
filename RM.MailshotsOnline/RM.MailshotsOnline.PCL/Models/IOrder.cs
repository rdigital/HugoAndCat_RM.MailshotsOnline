using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IOrder
    {
        Guid OrderId { get; set; }

        Guid CampaignId { get; set; }

        ICampaign Campaign { get; set; }

        DateTime CreatedDate { get; }

        DateTime? CompletionDate { get; set; }
    }
}
