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
    public class Mailshot : IMailshot
    {
        private MailshotContent _content;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MailshotId { get; set; }

        public Guid MailshotContentId { get; set; }

        [ForeignKey("MailshotContentId")]
        public virtual MailshotContent Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public bool Draft { get; set; }

        #region Explicit Interface Implementations
        
        IMailshotContent IMailshot.Content
        {
            get { return _content; }
            set { _content = (MailshotContent)value; }
        }

        #endregion 
    }
}
