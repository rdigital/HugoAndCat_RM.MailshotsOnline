using Glass.Mapper.Umb.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class CreateCanvas : BasePage
    {
        public BasePage MyCampaignsPage { get; set; }

        public BasePage LoginPage { get; set; }

        public BasePage CampaignHubPage { get; set; }

        public Guid? CampaignId { get; set; }
    }
}
