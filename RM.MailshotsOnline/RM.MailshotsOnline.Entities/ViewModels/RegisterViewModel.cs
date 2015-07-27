using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.Web.Attributes;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class RegisterViewModel
    {
        #region Part one

        [Required(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please re-enter a valid email address")]
        [Compare("Email", ErrorMessage = "The email addresses entered do not match. Please check and try again")]
        public string ConfirmEmail { get; set; }

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

        public ContactPreferences RoyalMailContactOptions { get; set; }

        public ContactPreferences ThirdPartyContactOptions { get; set; }
        
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept T&C agreement")]
        public bool AgreeToTermsAndConditions { get; set; }

        [Recaptcha]
        public bool IsRecaptchaValidated { get; set; }

        #endregion

        #region Part two

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


        #endregion
    }
}
