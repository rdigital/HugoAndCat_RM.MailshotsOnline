using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class MailshotViewModel
    {
        public Guid? MailshotId { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public bool Draft { get; set; }
    }
}
