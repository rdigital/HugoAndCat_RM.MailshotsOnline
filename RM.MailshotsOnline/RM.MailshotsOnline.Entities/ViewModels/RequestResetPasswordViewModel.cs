using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class RequestResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }
    }
}
