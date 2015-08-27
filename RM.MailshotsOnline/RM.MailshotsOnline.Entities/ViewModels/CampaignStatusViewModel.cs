using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class CampaignStatusViewModel
    {
        public bool DataSetsApproved { get; set; }

        public bool MailshotApproved { get; set; }

        public bool PostageSet { get; set; }

        public PCL.Enums.CampaignStatus CampaignStatus { get; set; }
    }
}
