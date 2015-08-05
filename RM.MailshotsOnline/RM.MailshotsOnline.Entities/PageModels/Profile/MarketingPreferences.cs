using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Umb.Configuration.Attributes;
using RM.MailshotsOnline.Entities.ViewModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.PageModels.Profile
{
    [UmbracoType(AutoMap = true)]
    public class MarketingPreferences : EditProfileBasePage
    {
        public string MainTitle { get; set; }

        public string RoyalMailDisclaimer { get; set; }

        public string RoyalMailTitle { get; set; }

        public string ThirdPartyDisclaimer { get; set; }

        public string ThirdPartyTitle { get; set; }
    }
}
