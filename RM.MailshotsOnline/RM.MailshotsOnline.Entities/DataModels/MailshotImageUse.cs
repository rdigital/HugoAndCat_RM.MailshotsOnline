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
    /// <summary>
    /// Model for recording the use of an image in a mailshot
    /// </summary>
    public class MailshotImageUse : IMailshotImageUse
    {
        /// <summary>
        /// The concrete model for the CMS image
        /// </summary>
        private CmsImage _cmsImage;

        /// <summary>
        /// The concrete model for the Mailshot
        /// </summary>
        private Mailshot _mailshot;

        /// <summary>
        /// Gets or sets the CMS Image being used in the mailshot
        /// </summary>
        [ForeignKey("CmsImageId")]
        public virtual CmsImage CmsImage
        {
            get { return _cmsImage; }
            set { _cmsImage = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the image being used in the mailshot
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public Guid CmsImageId { get; set; }

        /// <summary>
        /// Gets the date that the record was created
        /// </summary>
        public DateTime CreatedDate { get { return CreatedUtc; } }

        /// <summary>
        /// Gets or sets the date that the record was created (Database generated)
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        /// <summary>
        /// Gets or sets the Mailshot that uses the image
        /// </summary>
        [ForeignKey("MailshotId")]
        public virtual Mailshot Mailshot { get; set; }

        /// <summary>
        /// Gets or sets the ID of the mailshot that uses the image
        /// </summary>
        [Key]
        [Column(Order = 2)]
        public Guid MailshotId { get; set; }

        #region Explicit interface implementation

        /// <summary>
        /// Gets or sets the CMS Image being used in the mailshot
        /// </summary>
        ICmsImage IMailshotImageUse.CmsImage
        {
            get { return _cmsImage; }
            set { _cmsImage = (CmsImage)value; }
        }

        /// <summary>
        /// Gets or sets the Mailshot that uses the image
        /// </summary>
        IMailshot IMailshotImageUse.Mailshot
        {
            get { return _mailshot; }
            set { _mailshot = (Mailshot)value; }
        }

        #endregion
    }
}
