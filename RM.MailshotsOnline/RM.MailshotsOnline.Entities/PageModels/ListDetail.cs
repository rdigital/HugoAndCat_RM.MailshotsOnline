using Glass.Mapper.Umb.Configuration.Attributes;
using System;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class ListDetail : BasePage
    {
        public Guid DistributionListId { get; set; }

        public string BackLinkText { get; set; }

        public BasePage BackPage { get; set; }

        public string BackUrl { get; set; }

        public string CampaignId { get; set; }
    }
}
