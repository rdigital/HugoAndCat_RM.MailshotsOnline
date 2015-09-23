using System.Collections.Generic;

namespace RM.MailshotsOnline.PCL.Models.Reporting
{
    public interface IMembershipReport : IReport
    {
         IEnumerable<IMembershipReportEntity> Members { get; set; }
    }
}