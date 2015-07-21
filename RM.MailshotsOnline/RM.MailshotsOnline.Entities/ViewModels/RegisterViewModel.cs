using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ConfirmEmail { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PasswordReminder { get; set; }

        [Required]
        public ContactOptions RoyalMailContactOptions { get; set; }

        [Required]
        public ContactOptions ThirdPartyContactOptions { get; set; }

        [Required]
        public bool AgreeToTermsAndConditions { get; set; }

        public PageModels.Register PageModel { get; set; }
    }

    public class ContactOptions
    {
        public bool Post { get; set; }
        public bool Email { get; set; }
        public bool Telephone { get; set; }
        public bool SmsAndOther { get; set; }
    }
}
