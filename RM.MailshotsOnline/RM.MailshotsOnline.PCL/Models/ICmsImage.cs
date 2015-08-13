using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ICmsImage
    {
        Guid CmsImageId { get; set; }

        int UmbracoMediaId { get; set; }

        int? UserId { get; set; }

        string Src { get; set; }

        DateTime CreatedDate { get; }

        DateTime UpdatedDate { get; set; }

        ICollection<IMailshotImageUse> MailshotUses { get; set; }
    }
}
