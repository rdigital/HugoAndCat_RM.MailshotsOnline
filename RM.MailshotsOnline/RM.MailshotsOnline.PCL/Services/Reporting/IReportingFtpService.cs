using System.IO;

namespace RM.MailshotsOnline.PCL.Services.Reporting
{
    public interface IReportingFtpService
    {
        bool Put(Stream stream, string filePath);

        byte[] Get(string filePath);
    }
}
