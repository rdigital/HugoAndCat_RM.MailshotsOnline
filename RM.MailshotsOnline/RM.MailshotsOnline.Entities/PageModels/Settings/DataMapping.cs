using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace RM.MailshotsOnline.Entities.PageModels.Settings
{
    [UmbracoType]
    public class DataMapping : Item
    {
        private List<string> _mappings;

        [UmbracoProperty]
        public string FieldName { get; set; }
        [UmbracoProperty]
        public string TypicalFields { get; set; }

        public IEnumerable<string> FieldMappings
        {
            get
            {
                if (_mappings == null)
                {
                    _mappings = new List<string> {Name.ToLower(), FieldName.ToLower()};

                    if (!string.IsNullOrEmpty(TypicalFields))
                    {
                        _mappings.AddRange(TypicalFields.ToLower().Split(','));
                    }
                }

                return _mappings;
            }
        } 
    }
}
