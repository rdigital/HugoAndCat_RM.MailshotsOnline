using RM.MailshotsOnline.PCL;
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
    public class DataSearch : IDataSearch
    {
        private Campaign _campaign;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DataSearchId { get; set; }

        public string Name { get; set; }

        public string SearchCriteria { get; set; }

        public string ThirdPartyIdentifier { get; set; }

        public Enums.DataSearchStatus Status { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public Guid CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign Campaign
        {
            get { return _campaign; }
            set { _campaign = value; }
        }

        #region Explicit interfact implementation
        ICampaign IDataSearch.Campaign
        {
            get { return (ICampaign)_campaign; }
            set { _campaign = (Campaign)value; }
        }
        #endregion
    }
}
