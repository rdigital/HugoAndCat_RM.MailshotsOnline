using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using Glass.Mapper.Umb.DataMappers;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.PageModels
{
    [UmbracoType(AutoMap = true)]
    public class Lists : BasePage
    {
        // Content
        [UmbracoProperty]
        public string IntroText { get; set; }

        [UmbracoProperty]
        public string NoListsCopy { get; set; }

        // Buttons and Labels
        [UmbracoProperty]
        public string CreateFirstButtonText { get; set; }

        [UmbracoProperty]
        public string AddNewButtonText { get; set; }

        public BasePage CreatePage { get; set; }

        public string CreateLinkUrl { get; set; }

        public BasePage BackPage { get; set; }

        public string BackLinkUrl { get; set; }

        [UmbracoProperty]
        public string BackText { get; set; }

        // Service Data
        public IEnumerable<IDistributionList> UsersLists { get; set; }

        public IEnumerable<IDataSearch> RentedLists { get; set; }

        public ICampaign Campaign { get; set; }
    }
}
