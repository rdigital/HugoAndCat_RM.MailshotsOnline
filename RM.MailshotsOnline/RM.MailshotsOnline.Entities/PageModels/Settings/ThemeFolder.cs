using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels.Settings
{
    [UmbracoType(AutoMap = true)]
    public class ThemeFolder : Item
    {
        [UmbracoChildren(InferType = true)]
        public virtual IEnumerable<Theme> Themes { get; set; }
    }
}
