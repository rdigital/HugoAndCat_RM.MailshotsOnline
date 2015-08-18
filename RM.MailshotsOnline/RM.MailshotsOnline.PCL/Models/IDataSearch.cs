using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IDataSearch
    {
        Guid DataSearchId { get; set; }

        string Name { get; set; }

        DateTime CreatedDate { get; }

        //TODO: Finalise this
        string SearchCriteria { get; set; }

        //TODO: Finalise this
        string ThirdPartyIdentifier { get; set; }

        //TODO: Finalise this
        Enums.DataSearchStatus Status { get; set; }

        Guid CampaignId { get; set; }

        ICampaign Campaign { get; set; }
    }
}
