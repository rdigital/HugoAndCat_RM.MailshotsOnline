using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.MemberModels;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class MarketingPreferencesViewModel
    {
        public ContactPreferences RoyalMailContactPreferences { get; set; }

        public ContactPreferences ThirdPartyContactPreferences { get; set; }
    }
}
