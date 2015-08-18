using Newtonsoft.Json;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels
{
    [Table("Campaigns")]
    public class Campaign : ICampaign
    {
        /// <summary>
        /// The campaign's mailshot (concrete model)
        /// </summary>
        private Mailshot _mailshot;

        /// <summary>
        /// The campaign's data searches (concrete models)
        /// </summary>
        private ICollection<DataSearch> _dataSearches;

        /// <summary>
        /// The campaigns distribution list links (concrete models)
        /// </summary>
        private ICollection<CampaignDistributionList> _campaignDistributionLists;

        /// <summary>
        /// The ID of the campaign
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CampaignId { get; set; }

        /// <summary>
        /// The user's ID
        /// </summary>
        [JsonIgnore]
        [Index]
        public int UserId { get; set; }

        /// <summary>
        /// The date the campaign was created
        /// </summary>
        public DateTime CreatedDate { get { return CreatedUtc; } }

        /// <summary>
        /// The DB generated created date
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonIgnore]
        public DateTime CreatedUtc { get; private set; }

        /// <summary>
        /// The date the campaign was updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// The status of the campaign
        /// </summary>
        [DefaultValue(PCL.Enums.CampaignStatus.Draft)]
        public PCL.Enums.CampaignStatus Status { get; set; }

        /// <summary>
        /// The name of the campaign
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// User notes for the campaign
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// System notes for the campaign
        /// </summary>
        [JsonIgnore]
        public string SystemNotes { get; set; }

        /// <summary>
        /// ID of the campaign's mailshot
        /// </summary>
        public Guid MailshotId { get; set; }

        /// <summary>
        /// The campaign's mailshot
        /// </summary>
        [ForeignKey("MailshotId")]
        [JsonIgnore]
        public virtual Mailshot Mailshot
        {
            get { return _mailshot; }
            set { _mailshot = value; }
        }

        /// <summary>
        /// The data searches for the campaign
        /// </summary>
        [InverseProperty("Campaign")]
        [JsonIgnore]
        public virtual ICollection<DataSearch> DataSearches
        {
            get { return _dataSearches; }
            set { _dataSearches = value; }
        }

        /// <summary>
        /// Links to the distribution lists for the campaign
        /// </summary>
        [InverseProperty("Campaign")]
        [JsonIgnore]
        public virtual ICollection<CampaignDistributionList> CampaignDistributionLists
        {
            get { return _campaignDistributionLists; }
            set { _campaignDistributionLists = value; }
        }

        /// <summary>
        /// The distribution lists for the campaign
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public ICollection<DistributionList> DistributionLists
        {
            get { return (ICollection<DistributionList>)_campaignDistributionLists.Select(x => x.DistributionList); }
        }

        /// <summary>
        /// Indicates if a campaign has a mailshot set
        /// </summary>
        public bool HasMailshotSet
        {
            get { return this.Mailshot != null; }
        }

        /// <summary>
        /// Indicates if a campaign has any distribution lists assigned to it
        /// </summary>
        public bool HasDistributionLists
        {
            get { return this.CampaignDistributionLists.Any(); }
        }

        /// <summary>
        /// Indicates if a campaign has any data searches assigned to it
        /// </summary>
        public bool HasDataSearches
        {
            get { return this.DataSearches.Any(); }
        }

        #region Explicit interface definition

        IMailshot ICampaign.Mailshot
        {
            get { return (IMailshot)_mailshot; }
            set { _mailshot = (Mailshot)value; }
        }

        ICollection<IDataSearch> ICampaign.DataSearches
        {
            get { return (ICollection<IDataSearch>)_dataSearches; }
            set { _dataSearches = (ICollection<DataSearch>)value; }
        }

        ICollection<ICampaignDistributionList> ICampaign.CampaignDistributionLists
        {
            get { return (ICollection<ICampaignDistributionList>)_campaignDistributionLists; }
            set { _campaignDistributionLists = (ICollection<CampaignDistributionList>)value; }
        }

        ICollection<IDistributionList> ICampaign.DistributionLists
        {
            get { return (ICollection<IDistributionList>)_campaignDistributionLists.Select(x => x.DistributionList); }
        }

        #endregion
    }
}
