using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Entities.PageModels.Profile;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class MarketingPreferencesViewModel
    {
        public ContactOptions RoyalMailMarketingPreferences { get; set; }

        public ContactOptions ThirdPartyMarketingPreferences { get; set; }

        public MarketingPreferences PageModel { get; set; }
    }
}
