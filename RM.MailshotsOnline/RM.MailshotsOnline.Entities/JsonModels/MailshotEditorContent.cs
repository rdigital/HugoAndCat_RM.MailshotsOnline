using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.JsonModels
{
    public class MailshotEditorContent
    {
        public int FormatId { get; set; }

        public int TemplateId { get; set; }

        public int ThemeId { get; set; }

        public string Side { get; set; }

        public string Face { get; set; }

        public IEnumerable<ContentElement> Elements { get; set; }
    }
}
