using System.Xml;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMedia
    {
        int MediaId { get; set; }

        string Name { get; set; }

        int MailshotUses { get; set; }
    }
}