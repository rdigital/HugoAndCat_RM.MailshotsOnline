using System.Threading.Tasks;

namespace RM.MailshotsOnline.PCL.Services.Reporting
{
    public interface IReportingBlobService
    {
         string Store(byte[] bytes, string fileName, string mediaType);

         Task<string> StoreAsync(byte[] bytes, string fileName, string mediaType);
    }
}