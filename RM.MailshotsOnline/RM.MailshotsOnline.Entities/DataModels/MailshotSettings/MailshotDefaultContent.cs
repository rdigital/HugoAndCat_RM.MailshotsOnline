using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels.MailshotSettings
{
    [Table("MailshotDefaultContent")]
    public class MailshotDefaultContent : IMailshotDefaultContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MailshotDefaultContentId { get; set; }

        public string Name { get; set; }

        [Index]
        public int JsonIndex { get; set; }

        public string JsonData { get; set; }

        [Index]
        public int UmbracoPageId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }
    }
}
