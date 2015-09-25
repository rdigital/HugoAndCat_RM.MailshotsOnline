using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace RM.MailshotsOnline.Entities.PageModels.Settings
{
    [UmbracoType(AutoMap = true)]
    public class DataMappingFolder
    {
        [UmbracoChildren(InferType = true)]
        public virtual IEnumerable<DataMapping> Mappings { get; set; } 
    }
}
