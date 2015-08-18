using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ICampaignDistributionList
    {
        Guid CampaignId { get; set; }

        ICampaign Campaign { get; set; }

        Guid DistributionListId { get; set; }

        IDistributionList DistributionList { get; set; }

        DateTime CreatedDate { get; }
    }
}
