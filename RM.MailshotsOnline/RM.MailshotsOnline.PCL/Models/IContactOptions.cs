using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IContactOptions
    {
        bool Post { get; set; }

        bool Email { get; set; }

        bool Phone { get; set; }

        bool SmsAndOther { get; set; }
    }
}
