using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class Logout : BasePage
    {
        public string ThankYouLabel { get; set; }

        public string MainBody { get; set; }

        public string LoginCtaText { get; set; }

        public string HomepageCtaText { get; set; }
    }
}
