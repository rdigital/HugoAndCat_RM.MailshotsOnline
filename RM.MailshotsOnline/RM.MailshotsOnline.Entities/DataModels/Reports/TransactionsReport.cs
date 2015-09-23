using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Models.Reporting;

namespace RM.MailshotsOnline.Entities.DataModels.Reports
{
    public class TransactionsReport : Report, ITransactionsReport
    {
        public override DateTime CreatedDate { get; set; }

        public override string Name => "Transaction Report";

        public IEnumerable<ITransactionsReportEntity> Transactions { get; set; }
    }
}
