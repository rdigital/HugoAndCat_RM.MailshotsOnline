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
    public class Mailshot : IMailshot
    {
        [Key]
        public Guid MailshotId { get; set; }

        public Guid MailshotContentId { get; set; }

        [ForeignKey("MailshotContentId")]
        public virtual MailshotContent Content { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public bool Draft { get; set; }
    }
}
