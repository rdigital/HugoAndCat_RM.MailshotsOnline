using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ICampaign
    {
        Guid CampaignId { get; set; }

        int UserId { get; set; }

        int FormatId { get; set; }

        int TemplateId { get; set; }

        string Content { get; set; }

        DateTime CreatedDate { get; }

        Enums.CampaignStatus Status { get; set; }
    }
}
