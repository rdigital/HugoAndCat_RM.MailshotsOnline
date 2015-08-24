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
    [Table("Order")]
    public class Order : IOrder
    {
        private Campaign _campaign;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }

        public Guid CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign
        {
            get { return _campaign; }
            set { _campaign = value; }
        }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public DateTime? CompletionDate { get; set; }

        #region Explicit interface implementation

        ICampaign IOrder.Campaign
        {
            get { return _campaign; }
            set { _campaign = (Campaign)value; }
        }

        #endregion
    }
}
