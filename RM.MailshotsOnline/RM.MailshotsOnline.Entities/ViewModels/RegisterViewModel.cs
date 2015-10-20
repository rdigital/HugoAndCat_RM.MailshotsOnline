using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.Attributes;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class RegisterViewModel
    {
        #region Part one

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required(ErrorMessage = "Please enter a valid email address")]
        public string Email
        {
            get { return _email; }
            set { _email = value.ToLower(); }
        }

        private string _email;

        [Required(ErrorMessage = "Please re-enter a valid email address")]
        [CompareCaseInsensitive("Email", ErrorMessage = "The email addresses entered do not match. Please check and try again")]
        public string ConfirmEmail
        {
            get { return _confirmEmail; }
            set
            {
                _confirmEmail = value.ToLower();
            }
        }

        private string _confirmEmail;

        [Required(ErrorMessage = "Please select your title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an alphanumeric password of eight characters or more")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-type your password.")]
        [Compare("Password", ErrorMessage = "The passwords entered do not match. Please check and try again")]
        public string ConfirmPassword { get; set; }

        //public MarketingPreferencesViewModel MarketingPreferencesViewModel { get; set; }

        public bool AgreeToRoyalMailContact { get; set; }

        public bool AgreeToThirdPartyContact { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept T&C agreement")]
        public bool AgreeToTermsAndConditions { get; set; }

        [Recaptcha]
        public bool IsRecaptchaValidated { get; set; }

        #endregion

        #region Part two

        public OrganisationDetailsViewModel OrganisationDetailsViewModel { get; set; }

        #endregion
    }
}
