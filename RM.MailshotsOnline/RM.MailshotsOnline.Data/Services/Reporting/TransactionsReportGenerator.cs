using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.DataModels.Reports;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class TransactionsReportGenerator : ITransactionsReportGenerator
    {

        public TransactionsReportGenerator()
        {
        }

        public ITransactionsReport Generate()
        {
            throw new NotImplementedException();
        }
    }
}
