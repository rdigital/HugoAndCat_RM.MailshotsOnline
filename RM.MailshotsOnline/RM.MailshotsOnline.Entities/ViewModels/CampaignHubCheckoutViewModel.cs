using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class CampaignHubCheckoutViewModel
    {
        public CampaignHub PageModel { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the Terms and Conditions")]
        public bool AgreesToTermsAndConditions { get; set; }
    }
}
