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
    [Table("Formats")]
    public class Format : IFormat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FormatId { get; set; }

        [Index]
        public int JsonIndex { get; set; }

        public string Name { get; set; }

        [Index]
        public int UmbracoPageId { get; set; }

        public string XslData { get; set; }

        public string JsonData { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public decimal PricePerPrint { get; set; }
    }
}
