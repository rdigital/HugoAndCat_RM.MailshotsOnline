using RM.MailshotsOnline.Entities.PageModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class CaptureDetailsViewModel
    {
        public CaptureDetails PageModel { get; set; }

        [Required]
        public string OrganisationName { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string FlatNumber { get; set; }

        [Required]
        public string BuildingNumber { get; set; }

        public string BuildingName { get; set; }

        [Required]
        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string WorkPhone { get; set; }

        public string Mobile { get; set; }

        public string CampaignId { get; set; }
    }
}
