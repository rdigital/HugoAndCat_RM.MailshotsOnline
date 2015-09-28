using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using RM.MailshotsOnline.Data.Extensions;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using Umbraco.Core;
using Umbraco.Core.Services;
using System.Text;

namespace RM.MailshotsOnline.Data.Services
{
    public class MembershipService : IMembershipService
    {
        // this UmbracoMemberService could be replaced with a custom version, containing methods
        // that perform encryption of input values and decryption of output values.
        private static readonly IMemberService UmbracoMemberService = ApplicationContext.Current.Services.MemberService;

        private static ICryptographicService _cryptographicService;

        public MembershipService(ICryptographicService cryptographicService)
        {
            _cryptographicService = cryptographicService;
        }

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

            var securityMember = Membership.GetUser();

            if (securityMember != null)
            {
                var umbracoMember = UmbracoMemberService.GetByProviderKey(securityMember.ProviderUserKey);
                return umbracoMember.ToMemberEntityModel();
            }

            return null;
        }

        /// <summary>
        /// Create a new Umbraco Member given the domain entity and password.
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="password">The member's password</param>
        /// <returns></returns>
        public IMember CreateMember(IMember member, string password)
        {
            member.EmailAddress = member.EmailAddress.ToLower();

            if (UmbracoMemberService.Exists(member.EmailAddress))
            {
                return null;
            }

            var umbracoMember = UmbracoMemberService.CreateMemberWithIdentity(Guid.NewGuid().ToString(), member.EmailAddress, member.EmailAddress, "Member");

            umbracoMember = umbracoMember.UpdateValues(member);

            UmbracoMemberService.SavePassword(umbracoMember, password);
            UmbracoMemberService.Save(umbracoMember);

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
            var member = GetUmbracoMember(email);

            if (member != null)
            {
                var token = Guid.NewGuid();
                var expiryDays = int.Parse(ConfigurationManager.AppSettings["PasswordExpiryDays"]);

                member.SetValue("passwordResetToken", token.ToString());
                member.SetValue("passwordResetTokenExpiryDate", DateTime.UtcNow.AddDays(expiryDays).ToString(CultureInfo.InvariantCulture));
                UmbracoMemberService.Save(member);

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
            var umbracoMember = UmbracoMemberService.GetMembersByPropertyValue("passwordResetToken",
                token).FirstOrDefault();

            return ChangePassword(umbracoMember, password, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ChangePassword(string emailAddress, string password)
        {
            var umbracoMember = GetUmbracoMember(emailAddress);

            return ChangePassword(umbracoMember, password, false);
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

            var umbracoMember = UmbracoMemberService.GetMembersByPropertyValue("passwordResetToken", token).FirstOrDefault();

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
            try
            {
                var umbracoMember = GetUmbracoMember(member);
                UmbracoMemberService.SavePassword(umbracoMember, password);
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
        /// <param name="emailAddress">The member to update</param>
        /// <param name="member">The new set of details</param>
        /// <returns>Success</returns>
        public bool Save(string emailAddress, IMember member)
        {
            bool success = false;
            try
            {
                var umbracoMember = GetUmbracoMember(member);

                if (umbracoMember != null)
                {
                    umbracoMember = umbracoMember.UpdateValues(member);
                    UmbracoMemberService.Save(umbracoMember);

                    success = true;
                }
                else
                {
                    // Encrypt email
                    var emailSalt = _cryptographicService.GenerateEmailSalt(emailAddress);
                    var encryptedEmail = _cryptographicService.Encrypt(emailAddress, emailSalt);

                    umbracoMember = UmbracoMemberService.GetByEmail(encryptedEmail);

                    if (umbracoMember != null)
                    {
                        umbracoMember = umbracoMember.UpdateValues(member);
                        UmbracoMemberService.Save(umbracoMember);

                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return success;
        }

        /// <summary>
        /// Gets a list of all active members. 'Active' is defined as being approved.
        /// </summary>
        /// <returns>The list of all active members</returns>
        public IEnumerable<IMember> GetAllActiveMembers()
        {
            return UmbracoMemberService.GetAllMembers().Where(x => x.IsApproved).Select(x => x.ToMemberEntityModel());
        }

        /// <summary>
        /// Saves the new password against the member
        /// </summary>
        /// <param name="umbracoMember">Member to update the password for</param>
        /// <param name="password">The new password</param>
        /// <param name="clearPasswordResetToken">Indicates if the password reset token should be cleared</param>
        /// <returns>True on success</returns>
        private bool ChangePassword(Umbraco.Core.Models.IMember umbracoMember, string password, bool clearPasswordResetToken)
        {
            if (umbracoMember != null)
            {
                UmbracoMemberService.SavePassword(umbracoMember, password);

                if (clearPasswordResetToken)
                {
                    umbracoMember.SetValue("passwordResetToken", Guid.Empty.ToString());
                    umbracoMember.SetValue("passwordResetTokenExpiryDate",
                        DateTime.MinValue.ToString(CultureInfo.InvariantCulture));

                    UmbracoMemberService.Save(umbracoMember);
                }

                return true;
            }

            return false;
        }

        private Umbraco.Core.Models.IMember GetUmbracoMember(IMember member)
        {
            return
                UmbracoMemberService.GetByEmail(_cryptographicService.Encrypt(member.EmailAddress,
                    _cryptographicService.GenerateEmailSalt(member.EmailAddress)));
        }

        private Umbraco.Core.Models.IMember GetUmbracoMember(string plaintextEmail)
        {
            var encryptedEmail = _cryptographicService.EncryptEmailAddress(plaintextEmail);

            return UmbracoMemberService.GetByEmail(encryptedEmail);
        }
    }
}
