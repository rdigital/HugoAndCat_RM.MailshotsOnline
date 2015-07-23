using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.ViewModels;

namespace RM.MailshotsOnline.Entities.MemberModels
{
    public class Member : IMember, IContactable
    {
        public int Id { get; set; }

        public string EmailAddress { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public ContactPreferences RoyalMailMarketingPreferences { get; set; }

        public ContactPreferences ThirdPartyMarketingPreferencess { get; set; }
    }
}
