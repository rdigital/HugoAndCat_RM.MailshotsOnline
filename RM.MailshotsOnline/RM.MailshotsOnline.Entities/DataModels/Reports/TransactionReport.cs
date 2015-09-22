using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.DataModels.Reports
{
    public class TransactionReport : Report
    {
        public override DateTime CreatedDate { get; set; }

        public override string Name => "Transaction Report";

        public IEnumerable<TransactionReportEntity> Transactions { get; set; }
    }
}
