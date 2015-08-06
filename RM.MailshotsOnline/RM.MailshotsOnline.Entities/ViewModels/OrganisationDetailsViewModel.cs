using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.PageModels.Profile;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class OrganisationDetailsViewModel
    {
        [Required(ErrorMessage = "Please enter your postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Please enter your organisation name")]
        public string OrganisationName { get; set; }

        [Required(ErrorMessage = "Please enter your job title")]
        public string JobTitle { get; set; }

        public string FlatNumber { get; set; }

        public string BuildingNumber { get; set; }

        public string BuildingName { get; set; }

        [Required(ErrorMessage = "Please enter your street address")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "Please enter your city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter your country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter your work phone number")]
        public string WorkPhoneNumber { get; set; }

        public string MobilePhoneNumber { get; set; }

        public OrganisationDetails PageModel { get; set; }
    }
}
