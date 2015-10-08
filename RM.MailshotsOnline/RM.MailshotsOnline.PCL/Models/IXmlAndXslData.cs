using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IXmlAndXslData
    {
        string XmlData { get; set; }

        string XslStylesheet { get; set; }

        IEnumerable<string> UsedImageSrcs { get; set; }
    }
}
