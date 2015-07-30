using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter an alphanumeric password of eight characters or more")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-type your password.")]
        [Compare("Password", ErrorMessage = "The passwords entered do not match. Please check and try again")]
        public string ConfirmPassword { get; set; }
    }
}
