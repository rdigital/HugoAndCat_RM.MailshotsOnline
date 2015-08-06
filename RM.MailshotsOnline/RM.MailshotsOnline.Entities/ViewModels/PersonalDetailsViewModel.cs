using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using RM.MailshotsOnline.Entities.PageModels.Profile;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class PersonalDetailsViewModel
    {
        [Required(ErrorMessage = "Please select your title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }

        public PersonalDetails PageModel { get; set; }
    }
}
