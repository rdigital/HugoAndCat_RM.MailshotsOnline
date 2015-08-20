using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    /// <summary>
    /// Represents a direct marketing campaign
    /// </summary>
    public interface ICampaign
    {
        /// <summary>
        /// The ID of the campaign
        /// </summary>
        Guid CampaignId { get; set; }

        /// <summary>
        /// The user's ID
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// The date the campaign was created
        /// </summary>
        DateTime CreatedDate { get; }

        /// <summary>
        /// The date the campaign was updated
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// The status of the campaign
        /// </summary>
        Enums.CampaignStatus Status { get; set; }

        /// <summary>
        /// The name of the campaign
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// User notes for the campaign
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// System notes for the campaign
        /// </summary>
        string SystemNotes { get; set; }

        /// <summary>
        /// ID of the campaign's mailshot
        /// </summary>
        Guid? MailshotId { get; set; }

        /// <summary>
        /// The campaign's mailshot
        /// </summary>
        IMailshot Mailshot { get; set; }

        /// <summary>
        /// Links to the distribution lists for the campaign
        /// </summary>
        ICollection<ICampaignDistributionList> CampaignDistributionLists { get; set; }

        /// <summary>
        /// The distribution lists for the campaign
        /// </summary>
        ICollection<IDistributionList> DistributionLists { get; }

        /// <summary>
        /// The data searches for the campaign
        /// </summary>
        ICollection<IDataSearch> DataSearches { get; set; }
    }
}
