using Newtonsoft.Json;
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
    public class CmsImage : ICmsImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CmsImageId { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        [Required]
        public string Src { get; set; }

        [Required]
        public int UmbracoMediaId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int? UserId { get; set; }

        [InverseProperty("CmsImage")]
        public virtual ICollection<IMailshotImageUse> MailshotUses { get; set; }
    }
}
