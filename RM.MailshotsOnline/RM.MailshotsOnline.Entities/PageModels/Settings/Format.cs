using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels.Settings
{
    [UmbracoType(AutoMap = true)]
    public class Format : Item
    {
        public string Description { get; set; }

        public string XslData { get; set; }

        public int JsonIndex { get; set; }
    }
}
