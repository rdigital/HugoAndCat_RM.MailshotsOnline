using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.MemberModels
{
    public class ContactOptions : IContactOptions
    {
        public bool Post { get; set; }

        public bool Email { get; set; }

        public bool Phone { get; set; }

        public bool SmsAndOther { get; set; }
    }
}
