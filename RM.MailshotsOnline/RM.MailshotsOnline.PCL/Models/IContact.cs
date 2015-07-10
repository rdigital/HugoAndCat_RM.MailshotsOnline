using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IContact
    {
        Guid ContactId { get; set; }

        string SerialisedData { get; set; }

        Guid DistributionListId { get; set; }

        Enums.ContactStatus Status { get; set; }
    }
}
