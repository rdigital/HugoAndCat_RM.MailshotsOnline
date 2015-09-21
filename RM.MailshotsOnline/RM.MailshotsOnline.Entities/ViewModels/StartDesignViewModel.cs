using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class StartDesignViewModel
    {
        public CampaignHub PageModel { get; set; }

        public Guid CampaignId { get; set; }
    }
}
