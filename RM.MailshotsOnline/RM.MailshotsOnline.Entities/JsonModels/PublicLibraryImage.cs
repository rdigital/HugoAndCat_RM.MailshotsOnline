using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    [UmbracoType(AutoMap = true)]
    public class PublicLibraryImage : Image
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
