using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RM.MailshotsOnline.Data.Migrations;
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
                var expiryDays = int.Parse(ConfigurationManager.AppSettings["PasswordExpiryDays"]);

                member.SetValue("passwordResetToken", Guid.NewGuid().ToString());
                member.SetValue("passwordResetTokenExpiryDate", DateTime.UtcNow.AddSeconds(expiryDays).ToString(CultureInfo.InvariantCulture));
                Umbraco.Core.ApplicationContext.Current.Services.MemberService.Save(member);

                return token;
            }

            return null;
        }

        public void RedeemPasswordResetToken(string token, string password)
        {
            var memberService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            var umbracoMember = memberService.GetMembersByPropertyValue("passwordResetToken",
                token).FirstOrDefault();

            if (umbracoMember != null)
            {
                memberService.SavePassword(umbracoMember, password);
                umbracoMember.SetValue("passwordResetToken", Guid.Empty.ToString());
                umbracoMember.SetValue("passwordResetTokenExpiryDate", DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                memberService.Save(umbracoMember);
            }
        }

        public IMember GetMemberByPasswordResetToken(string token)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            var umbracoMember = membershipService.GetMembersByPropertyValue("passwordResetToken", token).FirstOrDefault();

            // if we're null at this point, then the token was old/spurious.
            if (umbracoMember == null)
            {
                return null;
            }

            var member = umbracoMember.ToMemberEntityModel();

            // check for token expiry. DateTime.MinValue represents an unset token.
            if (member.PasswordResetTokenExpiryDate != DateTime.MinValue &&
                member.PasswordResetTokenExpiryDate < DateTime.UtcNow)
            {
                return null;
            }

            return member;
        }

        public void SetNewPassword(IMember member, string password)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            var umbracoMember = membershipService.GetByEmail(member.EmailAddress);

            membershipService.SavePassword(umbracoMember, password);
        }
    }
}
