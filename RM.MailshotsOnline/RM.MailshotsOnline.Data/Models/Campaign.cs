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
    [Table("Campaigns")]
    public class Campaign : ICampaign
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CampaignId { get; set; }

        public int UserId { get; set; }

        public int FormatId { get; set; }

        public int TemplateId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public PCL.Enums.CampaignStatus Status { get; set; }

        public Guid MailshotId { get; set; }

        [ForeignKey("MailshotId")]
        public virtual Mailshot Mailshot { get; set; }
    }
}
