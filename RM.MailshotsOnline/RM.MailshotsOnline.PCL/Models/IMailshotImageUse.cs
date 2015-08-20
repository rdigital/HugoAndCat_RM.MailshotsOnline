using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    /// <summary>
    /// Model for recording the use of an image in a mailshot
    /// </summary>
    public interface IMailshotImageUse
    {
        /// <summary>
        /// Gets or sets the ID of the mailshot that uses the image
        /// </summary>
        Guid MailshotId { get; set; }

        /// <summary>
        /// Gets or sets the Mailshot that uses the image
        /// </summary>
        IMailshot Mailshot { get; set; }

        /// <summary>
        /// Gets or sets the ID of the image being used in the mailshot
        /// </summary>
        Guid CmsImageId { get; set; }

        /// <summary>
        /// Gets or sets the CMS Image being used in the mailshot
        /// </summary>
        ICmsImage CmsImage { get; set; }

        /// <summary>
        /// Gets the date that the record was created
        /// </summary>
        DateTime CreatedDate { get; }
    }
}
