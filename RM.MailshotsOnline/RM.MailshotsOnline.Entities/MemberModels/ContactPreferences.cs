using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.MemberModels
{
    public class ContactPreferences
    {
        public bool Post { get; set; }
        public bool Email { get; set; }
        public bool Telephone { get; set; }
        public bool SmsAndOther { get; set; }
    }
}
