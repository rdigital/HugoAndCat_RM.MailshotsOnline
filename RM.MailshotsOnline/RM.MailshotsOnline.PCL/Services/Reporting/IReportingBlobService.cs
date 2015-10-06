namespace RM.MailshotsOnline.PCL.Services.Reporting
{
    public interface IReportingBlobService
    {
         string Store(byte[] bytes, string fileName, string mediaType);
    }
}