using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class LibraryImage
    {
        public string Src { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
