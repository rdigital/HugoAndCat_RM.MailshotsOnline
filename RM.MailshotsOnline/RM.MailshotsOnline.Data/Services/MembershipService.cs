﻿using RM.MailshotsOnline.Data.Extensions;
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
        /// <summary>
        /// Retrieve the domain entity for the current user.
        /// </summary>
        /// <returns>The domain entity representing the current user.</returns>
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

        /// <summary>
        /// Create a new Umbraco Member given the domain entity and password.
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="password">The member's password</param>
        /// <returns></returns>
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

            membershipService.Save(umbracoMember);
            membershipService.SavePassword(umbracoMember, password);

            member.Id = umbracoMember.Id;

            return member;
        }

        /// <summary>
        /// Request that a password reset token be issued to a member
        /// </summary>
        /// <param name="email">The email address of the member</param>
        /// <returns>The token, if the email is valid. Null otherwise.</returns>
        public Guid? RequestPasswordReset(string email)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
            var member = membershipService.GetByEmail(email);

            if (member != null)
            {
                var token = Guid.NewGuid();
                var expiryDays = int.Parse(ConfigurationManager.AppSettings["PasswordExpiryDays"]);

                member.SetValue("passwordResetToken", token.ToString());
                member.SetValue("passwordResetTokenExpiryDate", DateTime.UtcNow.AddDays(expiryDays).ToString(CultureInfo.InvariantCulture));
                Umbraco.Core.ApplicationContext.Current.Services.MemberService.Save(member);

                return token;
            }

            return null;
        }

        /// <summary>
        /// Checks whether the given password reset token is valid.
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>False if the token is an empty Guid, or not at all.</returns>
        private bool IsPasswordResetTokenValid(string token)
        {
            // if we've been given an empty guid, or an invalid token
            var guidToken = new Guid();
            if (!Guid.TryParse(token, out guidToken) || guidToken.Equals(Guid.Empty))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set a new password for the token holder, provided the token is valid
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="password">The new password</param>
        /// <returns>Success</returns>
        public bool RedeemPasswordResetToken(string token, string password)
        {
            // if the token isn't a guid, fail immediately.
            if (!IsPasswordResetTokenValid(token))
            {
                return false;
            }

            // else proceed in trying to get the member based on the token
            var memberService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
            var umbracoMember = memberService.GetMembersByPropertyValue("passwordResetToken",
                token).FirstOrDefault();

            // if there is a result, set that member's password
            if (umbracoMember != null)
            {
                memberService.SavePassword(umbracoMember, password);
                umbracoMember.SetValue("passwordResetToken", Guid.Empty.ToString());
                umbracoMember.SetValue("passwordResetTokenExpiryDate", DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                memberService.Save(umbracoMember);

                return true;
            }

            // ... we didn't find a member using that guid
            return false;
        }

        /// <summary>
        /// Retrieve a member by password reset token
        /// </summary>
        /// <param name="token">The password reset token in the form a GUID</param>
        /// <returns></returns>
        public IMember GetMemberByPasswordResetToken(string token)
        {
            if (!IsPasswordResetTokenValid(token))
            {
                return null;
            }

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

        /// <summary>
        /// Sets a new password for the given member
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="password">The new password to set</param>
        public bool SetNewPassword(IMember member, string password)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            try
            {
                var umbracoMember = membershipService.GetByEmail(member.EmailAddress);
                membershipService.SavePassword(umbracoMember, password);
            }
            catch
            {
                // Probably do some logging here.
                return false;
            }

            return true;
        }

        /// <summary>
        /// Overwrite an existing member record with the provided one.
        /// </summary>
        /// <param name="member">The member to update</param>
        /// <returns>Success</returns>
        public bool Save(IMember member)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
            var umbracoMember = membershipService.GetByEmail(member.EmailAddress);

            if (umbracoMember != null)
            {
                umbracoMember = umbracoMember.UpdateValues(member);
                membershipService.Save(umbracoMember);

                return true;
            }

            return false;
        }
    }
}
