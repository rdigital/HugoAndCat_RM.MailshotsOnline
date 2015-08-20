using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    /// <summary>
    /// Model for storing mailshot JSON content
    /// </summary>
    public interface IMailshotContent
    {
        /// <summary>
        /// Gets or sets the ID of the mailshot content
        /// </summary>
        Guid MailshotContentId { get; set; }

        /// <summary>
        /// Gets or sets the mailshot JSON content
        /// </summary>
        string Content { get; set; }
    }
}
