using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class ContentElement
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public IEnumerable<ContentStyle> Styles { get; set; }
    }
}
