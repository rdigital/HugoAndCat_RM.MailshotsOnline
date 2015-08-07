using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.PageModels.Profile;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter an alphanumeric password of eight characters or more")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please re-type your password.")]
        [Compare("NewPassword", ErrorMessage = "The passwords entered do not match. Please check and try again")]
        public string ConfirmNewPassword { get; set; }

        public PersonalDetails PageModel { get; set; }
    }
}
