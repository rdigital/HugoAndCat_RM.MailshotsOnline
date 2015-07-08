using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IPostalOption
    {
        Guid PostalOptionId { get; set; }

        int FormatId { get; set; }

        string Name { get; set; }

        string Currency { get; set; }

        decimal PricePerUnit { get; set; }

        decimal Tax { get; set; }

        string TaxCode { get; set; }
    }
}
