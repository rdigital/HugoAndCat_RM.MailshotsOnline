using RM.MailshotsOnline.PCL.Models.Reporting;

namespace RM.MailshotsOnline.PCL.Services.Reporting
{
    public interface IReportingService
    {
        IMembershipReportGenerator MembershipReportGenerator { get; }

        ITransactionsReportGenerator TransactionsReportGenerator { get; }
    }
}