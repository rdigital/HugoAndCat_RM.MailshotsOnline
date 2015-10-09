using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    /// <summary>
    /// Model representing Mailshot data
    /// </summary>
    public interface IMailshot
    {
        /// <summary>
        /// Gets or sets the ID of the Mailshot
        /// </summary>
        Guid MailshotId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the mailshot content
        /// </summary>
        Guid? MailshotContentId { get; set; }

        /// <summary>
        /// Gets or sets the Mailshot Content
        /// </summary>
        IMailshotContent Content { get; set; }

        /// <summary>
        /// Gets or sets the content text
        /// </summary>
        string ContentText { get; set; }

        /// <summary>
        /// Gets or sets the content blob ID
        /// </summary>
        string ContentBlobId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who owns the Mailshot
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Mailshot
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the date that the Mailshot was updated
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date the Mailshot was created
        /// </summary>
        DateTime CreatedDate { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the Mailshot is still in draft
        /// </summary>
        bool Draft { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Format
        /// </summary>
        Guid FormatId { get; set; }

        /// <summary>
        /// Gets or sets the Format
        /// </summary>
        IFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Template
        /// </summary>
        Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the Template
        /// </summary>
        ITemplate Template { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Theme
        /// </summary>
        Guid ThemeId { get; set; }

        /// <summary>
        /// Gets or sets the Theme
        /// </summary>
        ITheme Theme { get; set; }

        /// <summary>
        /// Gets or sets the Blob ID of the proof PDF
        /// </summary>
        string ProofPdfBlobId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the proof PDF
        /// </summary>
        string ProofPdfUrl { get; set; }

        /// <summary>
        /// Gets or sets the Sparq order number of the proof PDF
        /// </summary>
        Guid ProofPdfOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the status of the proof PDF render job
        /// </summary>
        Enums.PdfRenderStatus ProofPdfStatus { get; set; }

        /// <summary>
        /// Gets or sets the associated campaigns for this mailshot
        /// </summary>
        ICollection<ICampaign> Campaigns { get; set; }
    }
}
