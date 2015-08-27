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
        /// The chosen postal option (concrete model)
        /// </summary>
        private PostalOption _postalOption;

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
        public Guid? MailshotId { get; set; }

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

        public string MailshotTitle
        {
            get
            {
                if (_mailshot != null)
                {
                    return _mailshot.Name;
                }

                return null;
            }
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
        public IEnumerable<DistributionList> DistributionLists
        {
            get
            {
                if (_campaignDistributionLists != null)
                {
                    return _campaignDistributionLists.Select(x => x.DistributionList);
                }

                return null;
            }
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
            get
            {
                if (this.CampaignDistributionLists != null)
                {
                    return this.CampaignDistributionLists.Any();
                }

                return false;
            }
        }

        /// <summary>
        /// Indicates if a campaign has any data searches assigned to it
        /// </summary>
        public bool HasDataSearches
        {
            get
            {
                if (this.DataSearches != null)
                {
                    return this.DataSearches.Any();
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the ID of the chosen postal option
        /// </summary>
        public Guid? PostalOptionId { get; set; }

        /// <summary>
        /// Gets or sets the postal option
        /// </summary>
        public PostalOption PostalOption { get; set; }

        [NotMapped]
        /// <summary>
        /// Gets the number of recipients (of own data) the Campaign will be sent to
        /// </summary>
        public int OwnDataRecipientCount
        {
            get
            {
                if (HasDistributionLists)
                {
                    return this.DistributionLists.Sum(dl => dl.RecordCount);
                }

                return 0;
            }
        }

        [NotMapped]
        /// <summary>
        /// Gets the number of recipients (or rented data) the Campaign will be sent to
        /// </summary>
        public int RentedDataRecipientCount
        {
            get
            {
                if (HasDataSearches)
                {
                    return this.DataSearches.Sum(ds => ds.RecordCount);
                }

                return 0;
            }
        }

        [NotMapped]
        /// <summary>
        /// Gets the total combined number of recipients
        /// </summary>
        public int TotalRecipientCount
        {
            get
            {
                return this.RentedDataRecipientCount + this.OwnDataRecipientCount;
            }
        }

        [DefaultValue(false)]
        /// <summary>
        /// Gets or sets a value indicating if the user has approved the data sets for the campaign
        /// </summary>
        public bool DataSetsApproved { get; set; }

        /// <summary>
        /// Gets a value indicating if the Campaign's mailshot design is approved
        /// </summary>
        public bool MailshotApproved
        {
            get
            {
                if (Mailshot != null)
                {
                    return !Mailshot.Draft;
                }

                return false;
            }
        }

        #region Explicit interface definition

        IMailshot ICampaign.Mailshot
        {
            get { return (IMailshot)_mailshot; }
            set { _mailshot = (Mailshot)value; }
        }

        ICollection<IDataSearch> ICampaign.DataSearches
        {
            get
            {
                var newSet = _dataSearches.Cast<IDataSearch>().ToList<IDataSearch>();
                return newSet;
            }
            set { _dataSearches = (ICollection<DataSearch>)value; }
        }

        ICollection<ICampaignDistributionList> ICampaign.CampaignDistributionLists
        {
            get { return (ICollection<ICampaignDistributionList>)_campaignDistributionLists; }
            set { _campaignDistributionLists = (ICollection<CampaignDistributionList>)value; }
        }

        IEnumerable<IDistributionList> ICampaign.DistributionLists
        {
            get
            {
                if (_campaignDistributionLists != null)
                {
                    return _campaignDistributionLists.Select(x => x.DistributionList);
                }

                return null;
            }
        }

        IPostalOption ICampaign.PostalOption
        {
            get { return _postalOption; }
            set { _postalOption = (PostalOption)value; }
        }

        #endregion
    }
}
