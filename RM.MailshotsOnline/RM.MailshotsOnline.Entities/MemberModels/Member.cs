using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
