using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email
        {
            get { return _email; }
            set { _email = value.ToLower(); }
        }

        private string _email;

        [Required]
        public string Password { get; set; }

        public PageModels.Login PageModel { get; set; }
    }
}
