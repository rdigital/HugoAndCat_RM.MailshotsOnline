using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IContact
    {
        Guid ContactId { get; set; }

        string Name { get; set; }

        string Address1 { get; set; }

        string Address2 { get; set; }

        string Address3 { get; set; }

        string Postcode { get; set; }

        string Country { get; set; }

        Guid DistributionListId { get; set; }

        Enums.ContactStatus Status { get; set; }
    }
}
