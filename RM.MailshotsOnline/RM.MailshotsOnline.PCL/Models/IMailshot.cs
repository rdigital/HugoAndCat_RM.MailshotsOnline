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

        int FormatId { get; set; }

        int TemplateId { get; set; }

        int ThemeId { get; set; }
    }
}
