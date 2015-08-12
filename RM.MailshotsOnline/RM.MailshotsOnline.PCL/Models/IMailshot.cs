using RM.MailshotsOnline.PCL.Models.MailshotSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMailshot
    {
        Guid MailshotId { get; set; }

        Guid MailshotContentId { get; set; }

        IMailshotContent Content { get; set; }

        int UserId { get; set; }

        string Name { get; set; }

        DateTime UpdatedDate { get; set; }

        DateTime CreatedDate { get; }

        bool Draft { get; set; }

        Guid FormatId { get; set; }

        IFormat Format { get; set; }

        Guid TemplateId { get; set; }

        ITemplate Template { get; set; }

        Guid ThemeId { get; set; }

        ITheme Theme { get; set; }

        string ProofPdfBlobId { get; set; }

        string ProofPdfUrl { get; set; }

        Guid ProofPdfOrderNumber { get; set; }

        Enums.PdfRenderStatus ProofPdfStatus { get; set; }
    }
}
