using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class RegisterOrganisationDetailsViewModel
    {
        [Required]
        public string PostCode { get; set; }

        [Required]
        public string PartOne { get; set; }
    }
}
