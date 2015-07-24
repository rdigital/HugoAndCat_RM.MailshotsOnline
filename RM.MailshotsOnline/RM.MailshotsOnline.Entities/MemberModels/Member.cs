using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.ViewModels;

namespace RM.MailshotsOnline.Entities.MemberModels
{
    public class Member : IMember
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public bool CanWeContactByPost { get; set; }

        public bool CanWeContactByEmail { get; set; }

        public bool CanWeContactByPhone { get; set; }

        public bool CanWeContactBySmsAndOther { get; set; }

        public bool CanThirdPatiesContactByPost { get; set; }

        public bool CanThirdPatiesContactByEmail { get; set; }

        public bool CanThirdPatiesContactByPhone { get; set; }

        public bool CanThirdPatiesContactBySmsAndOther { get; set; }
    }
}
