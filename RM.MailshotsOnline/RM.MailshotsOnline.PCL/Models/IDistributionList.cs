using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IDistributionList
    {
        Guid DistributionListId { get; set; }

        string Name { get; set; }

        int UserId { get; set; }

        DateTime CreatedDate { get; }

        ICollection<IContact> Contacts { get; set; }

        int RecordCount { get; set; }
    }
}
