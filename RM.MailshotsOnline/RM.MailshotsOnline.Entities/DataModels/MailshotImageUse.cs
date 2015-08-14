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
    public class MailshotImageUse : IMailshotImageUse
    {
        private CmsImage _cmsImage;

        private Mailshot _mailshot;

        [ForeignKey("CmsImageId")]
        public virtual CmsImage CmsImage
        {
            get { return _cmsImage; }
            set { _cmsImage = value; }
        }

        [Key]
        [Column(Order = 1)]
        public Guid CmsImageId { get; set; }

        public DateTime CreatedDate { get { return CreatedUtc; } }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        [ForeignKey("MailshotId")]
        public virtual Mailshot Mailshot { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid MailshotId { get; set; }

        #region Explicit interface implementation

        ICmsImage IMailshotImageUse.CmsImage
        {
            get { return _cmsImage; }
            set { _cmsImage = (CmsImage)value; }
        }

        IMailshot IMailshotImageUse.Mailshot
        {
            get { return _mailshot; }
            set { _mailshot = (Mailshot)value; }
        }

        #endregion
    }
}
