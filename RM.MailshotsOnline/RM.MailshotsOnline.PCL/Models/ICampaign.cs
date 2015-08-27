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
        IEnumerable<IDistributionList> DistributionLists { get; }

        /// <summary>
        /// The data searches for the campaign
        /// </summary>
        ICollection<IDataSearch> DataSearches { get; set; }

        /// <summary>
        /// Gets or sets the ID of the chosen postal option
        /// </summary>
        Guid? PostalOptionId { get; set; }

        /// <summary>
        /// Gets or sets the postal option
        /// </summary>
        IPostalOption PostalOption { get; set; }

        /// <summary>
        /// Indicates if a campaign has a mailshot set
        /// </summary>
        bool HasMailshotSet { get; }

        /// <summary>
        /// Indicates if a campaign has any distribution lists assigned to it
        /// </summary>
        bool HasDistributionLists { get; }

        /// <summary>
        /// Indicates if a campaign has any data searches assigned to it
        /// </summary>
        bool HasDataSearches { get; }

        /// <summary>
        /// Gets the number of recipients (of own data) the Campaign will be sent to
        /// </summary>
        int OwnDataRecipientCount { get; }

        /// <summary>
        /// Gets the number of recipients (or rented data) the Campaign will be sent to
        /// </summary>
        int RentedDataRecipientCount { get; }

        /// <summary>
        /// Gets the total combined number of recipients
        /// </summary>
        int TotalRecipientCount { get; }

        /// <summary>
        /// Gets or sets a value indicating if the user has approved the data sets for the campaign
        /// </summary>
        bool DataSetsApproved { get; set; }

        /// <summary>
        /// Gets a value indicating if the Campaign's mailshot design is approved
        /// </summary>
        bool MailshotApproved { get; }
    }
}
