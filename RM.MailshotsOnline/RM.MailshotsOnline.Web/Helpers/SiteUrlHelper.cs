using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Helpers
{
    public static class SiteUrlHelper
    {
        public static string GetUrlForCampaign(Guid campaignId)
        {
            return string.Format("/campaigns/campaign-hub/?campaignId={0}", campaignId);
        }

        public static string GetUrlForCampaignData(Guid campaignId)
        {
            return string.Format("/campaigns/{0}/data", campaignId);
        }

        public static string GetUrlForCampaignDataCreate(Guid campaignId)
        {
            return string.Format("/campaigns/{0}/data/create", campaignId);
        }
    }
}
