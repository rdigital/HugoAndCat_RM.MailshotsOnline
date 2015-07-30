using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class PdfRenderJobViewModel
    {
        public string AuthenticationKey { get; set; }

        public string OrderId { get; set; }

        public List<PdfRenderResultViewModel> Results { get; set; }
    }
}
