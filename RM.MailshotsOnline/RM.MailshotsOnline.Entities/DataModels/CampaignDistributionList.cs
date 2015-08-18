using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels
{
    public class CampaignDistributionList : ICampaignDistributionList
    {
        private Campaign _campaign;

        private DistributionList _distributionList;

        [Key]
        [Column(Order = 1)]
        public Guid CampaignId { get; set; }

        public Campaign Campaign
        {
            get { return _campaign; }
            set { _campaign = value; }
        }

        [Key]
        [Column(Order = 2)]
        public Guid DistributionListId { get; set; }

        public DistributionList DistributionList
        {
            get { return _distributionList; }
            set { _distributionList = value; }
        }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        #region Explicit interface implementation

        ICampaign ICampaignDistributionList.Campaign
        {
            get { return (ICampaign)_campaign; }
            set { _campaign = (Campaign)value; }
        }

        IDistributionList ICampaignDistributionList.DistributionList
        {
            get { return (IDistributionList)_distributionList; }
            set { _distributionList = (DistributionList)value; }
        }

        #endregion
    }
}
