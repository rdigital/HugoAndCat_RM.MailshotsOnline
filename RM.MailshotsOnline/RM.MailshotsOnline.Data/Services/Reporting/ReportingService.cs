using System;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class ReportingService
    {
        public static IMembershipReportGenerator MembershipReportGenerator;
        public static ITransactionsReportGenerator TransactionsReportGenerator;

        public ReportingService(IMembershipReportGenerator membershipReportGenerator, ITransactionsReportGenerator transactionsReportGenerator)
        {
            MembershipReportGenerator = membershipReportGenerator;
            TransactionsReportGenerator = transactionsReportGenerator;
        }
    }
}
