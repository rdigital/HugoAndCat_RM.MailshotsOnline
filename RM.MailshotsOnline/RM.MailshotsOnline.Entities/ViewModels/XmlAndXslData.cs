using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class XmlAndXslData
    {
        public string XmlData { get; set; }

        public string XslStylesheet { get; set; }

        public IEnumerable<string> UsedImageSrcs { get; set; }
    }
}
