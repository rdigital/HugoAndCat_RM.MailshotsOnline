using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Test.Mocks
{
    public class MockMembershipService : IMembershipService
    {
        public PCL.Models.IMember GetCurrentMember()
        {
            return new Member
            {
                Id = 1,
                EmailAddress = "ext-mradford@hugoandcat.com",
                FirstName = "Mark",
                LastName = "Radford",
                Title = "Mr",
                IsApproved = true,
                IsLockedOut = false
            };
        }

        public IMember CreateMember(IMember member, string password)
        {
            throw new NotImplementedException();
        }

        public Guid? RequestPasswordReset(string email)
        {
            throw new NotImplementedException();
        }
    }
}
