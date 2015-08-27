using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Web.Extensions
{
    public static class ModelExtensions
    {
        public static string GetLastEditedSummary(this ICampaign campaign)
        {
            return string.Format("Last edited at {0:dd/MM/yyyy} at {0:hh:mm tt}", campaign.UpdatedDate);
        }

        public static bool HasData(this ICampaign campaign)
        {
            return campaign.HasDataSearches || campaign.HasDistributionLists;
        }

        public static bool NeedsDataAndDesign(this ICampaign campaign)
        {
            return !campaign.HasMailshotSet && !campaign.HasDataSearches && !campaign.HasDistributionLists;
        }
    }
}
