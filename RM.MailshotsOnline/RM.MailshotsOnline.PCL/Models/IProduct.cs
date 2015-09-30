using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IProduct
    {
        string ProductSku { get; set; }

        string Name { get; set; }

        string Category { get; set; }

        DateTime CreatedDate { get; }

        DateTime UpdatedDate { get; set; }
    }
}
