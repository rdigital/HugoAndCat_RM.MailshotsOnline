using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class PdfRenderResultViewModel
    {
        public string Status { get; set; }

        public string BlobId { get; set; }

        public string Errors { get; set; }
    }
}
