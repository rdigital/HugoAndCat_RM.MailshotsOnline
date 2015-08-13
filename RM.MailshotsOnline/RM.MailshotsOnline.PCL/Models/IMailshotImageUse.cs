using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMailshotImageUse
    {
        Guid MailshotImageUseId { get; set; }

        Guid MailshotId { get; set; }

        IMailshot Mailshot { get; set; }

        Guid CmsImageId { get; set; }

        ICmsImage CmsImage { get; set; }

        DateTime CreatedDate { get; }
    }
}
