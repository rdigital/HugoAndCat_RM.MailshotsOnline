using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing.Constraints;
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

        public IMember GetMemberById(int id)
        {
            return new Member
            {
                Id = id,
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

        public bool RedeemPasswordResetToken(string token, string password)
        {
            throw new NotImplementedException();
        }

        public IMember GetMemberByPasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }

        public bool SetNewPassword(IMember member, string password)
        {
            throw new NotImplementedException();
        }

        public bool Save(string emailAddress, IMember member)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMember> GetAllActiveMembers()
        {
            throw new NotImplementedException();
        }
    }
}
