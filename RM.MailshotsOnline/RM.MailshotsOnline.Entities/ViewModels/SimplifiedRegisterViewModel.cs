using RM.MailshotsOnline.Entities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class SimplifiedRegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a valid email address")]
        public string Email
        {
            get { return _email; }
            set { _email = value.ToLower(); }
        }

        private string _email;

        [Required(ErrorMessage = "Please select your title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an alphanumeric password of eight characters or more")]
        public string Password { get; set; }

        public bool AgreeToRoyalMailContact { get; set; }

        public bool AgreeToThirdPartyContact { get; set; }

        [Recaptcha]
        public bool IsRecaptchaValidated { get; set; }
    }
}
