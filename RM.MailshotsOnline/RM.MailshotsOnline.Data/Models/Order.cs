using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Models
{
    [Table("Order")]
    public class Order : IOrder
    {
        private ICollection<DistributionList> _distributionLists;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }

        public Guid CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }

        public Guid PostalOptionId { get; set; }

        [ForeignKey("PostalOptionId")]
        public virtual PostalOption PostalOption { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public DateTime? CompletionDate { get; set; }

        public ICollection<DistributionList> DistributionLists
        {
            get { return _distributionLists; }
            set { _distributionLists = value; }
        }

        #region Explicit Interface Implementations
        ICollection<IDistributionList> IOrder.DistributionLists
        {
            get { return (ICollection<IDistributionList>)_distributionLists; }
            set { _distributionLists = (ICollection<DistributionList>)value; }
        }
        #endregion
    }
}
