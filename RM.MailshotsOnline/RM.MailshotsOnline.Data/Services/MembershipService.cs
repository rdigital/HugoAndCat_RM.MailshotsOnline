using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Persistence.Querying;

namespace RM.MailshotsOnline.Data.Services
{
    public class MembershipService : IMembershipService
    {
        public IMember GetCurrentMember()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var securityMember = System.Web.Security.Membership.GetUser();

            var umbracoMember = Umbraco.Core.ApplicationContext.Current.Services.MemberService.GetByProviderKey(securityMember.ProviderUserKey);

            return umbracoMember.ToMemberEntityModel();
        }

        public IMember CreateMember(IMember member, string password)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            if (membershipService.Exists(member.EmailAddress))
            {
                return null;
            }

            var umbracoMember = membershipService.CreateMemberWithIdentity(member.EmailAddress, member.EmailAddress,
                member.EmailAddress, "Member");

            umbracoMember = umbracoMember.UpdateValues(member);

            Umbraco.Core.ApplicationContext.Current.Services.MemberService.Save(umbracoMember);
            Umbraco.Core.ApplicationContext.Current.Services.MemberService.SavePassword(umbracoMember, password);

            member.Id = umbracoMember.Id;

            return member;
        }

        public Guid? RequestPasswordReset(string email)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            var member = membershipService.GetByEmail(email);

            if (member != null)
            {
                var token = Guid.NewGuid();

                member.SetValue("passwordResetToken", token.ToString());
                Umbraco.Core.ApplicationContext.Current.Services.MemberService.Save(member);

                return token;
            }

            return null;
        }

        public IMember GetMemberByPasswordResetToken(string token)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            var member = membershipService.GetMembersByPropertyValue("passwordResetToken", token).FirstOrDefault();

            return member.ToMemberEntityModel();
        }

        public void SetNewPassword(IMember member, string password)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            var umbracoMember = membershipService.GetByEmail(member.EmailAddress);

            membershipService.SavePassword(umbracoMember, password);
        }
    }
}
