
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class Media : IMedia
    {
        public int MediaId { get; set; }

        public string Name { get; set; }

        public int MailshotUses { get; set; }
    }
}