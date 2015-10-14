using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class MyCampaigns : BasePage
    {
        public string MainContent { get; set; }

        public string DefaultTitleForNewCampaigns { get; set; }

        public BasePage CampaignHubPage { get; set; }
    }
}
