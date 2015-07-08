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

        DateTime CreatedDate { get; }

        DateTime? CompletionDate { get; set; }

        Guid PostalOptionId { get; set; }

        ICollection<IDistributionList> DistributionLists { get; set; }
    }
}
