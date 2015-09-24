using System.Collections.Generic;

namespace RM.MailshotsOnline.PCL.Models.Reporting
{
    public interface ITransactionsReport : IReport
    {
        IEnumerable<ITransactionsReportEntity> Transactions { get; set; }
    }
}
