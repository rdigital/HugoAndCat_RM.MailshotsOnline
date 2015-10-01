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

        int RecordCount { get; set; }

        string BlobFinal { get; set; }

        DateTime UpdatedDate { get; set; }

        string BlobWorking { get; set; }

        string BlobErrors { get; set; }

        byte[] DataSalt { get; }

        Enums.DistributionListState ListState { get; set; }
    }
}
