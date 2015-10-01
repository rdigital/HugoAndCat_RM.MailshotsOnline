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
        private ICollection<MailshotImageUse> _mailshotUses;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CmsImageId { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        [Required]
        [MaxLength(2048)]
        public string Src { get; set; }

        [Required]
        public int UmbracoMediaId { get; set; }

        public DateTime UpdatedDate { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        [InverseProperty("CmsImage")]
        public virtual ICollection<MailshotImageUse> MailshotUses
        {
            get { return _mailshotUses; }
            set { _mailshotUses = value; }
        }

        ICollection<IMailshotImageUse> ICmsImage.MailshotUses
        {
            get { return (ICollection<IMailshotImageUse>)_mailshotUses; }
            set { _mailshotUses = (ICollection<MailshotImageUse>)value; }
        }
    }
}
