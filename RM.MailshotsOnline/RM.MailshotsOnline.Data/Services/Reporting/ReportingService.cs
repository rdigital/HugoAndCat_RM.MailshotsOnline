using System;
using System.Collections.Generic;
using RM.MailshotsOnline.Entities.DataModels.Reports;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class ReportingService : IReportingService
    {
        public IMembershipReportGenerator MembershipReportGenerator { get; }

        public ITransactionsReportGenerator TransactionsReportGenerator { get; }

        public ReportingService(IMembershipReportGenerator membershipReportGenerator, ITransactionsReportGenerator transactionsReportGenerator)
        {
            MembershipReportGenerator = membershipReportGenerator;
            TransactionsReportGenerator = transactionsReportGenerator;
        }
    }
}
