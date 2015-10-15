using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IMembershipService
    {
        /// <summary>
        /// Retrieve the domain entity for the current user.
        /// </summary>
        /// <returns>The domain entity representing the current user.</returns>
        IMember GetCurrentMember();

        /// <summary>
        /// Create a new Umbraco Member given the domain entity and password.
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="password">The member's password</param>
        /// <returns></returns>
        IMember CreateMember(IMember member, string password);

        /// <summary>
        /// Request that a password reset token be issued to a member
        /// </summary>
        /// <param name="email">The email address of the member</param>
        /// <returns>The token, if the email is valid. Null otherwise.</returns>
        Guid? RequestPasswordReset(string email);

        /// <summary>
        /// Set a new password for the token holder, provided the token is valid
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="password">The new password</param>
        /// <returns>Success/failure</returns>
        bool RedeemPasswordResetToken(string token, string password);

        /// <summary>
        /// Retrieve a member by password reset token
        /// </summary>
        /// <param name="token">The password reset token in the form a GUID</param>
        /// <returns>The member</returns>
        IMember GetMemberByPasswordResetToken(string token);

        /// <summary>
        /// Gets a member by their Umbraco ID
        /// </summary>
        /// <param name="id">Umbraco ID of the member</param>
        /// <returns>Member object</returns>
        IMember GetMemberById(int id);

        /// <summary>
        /// Sets a new password for the given member
        /// </summary>
        /// <param name="member">The member</param>
        /// <param name="password">The new password to set</param>
        bool SetNewPassword(IMember member, string password);

        /// <summary>
        /// Overwrite an existing member record with the provided one.
        /// </summary>
        /// <param name="emailAddress">The member to update</param>
        /// <param name="member">The new set of details</param>
        /// <returns>Success/failure</returns>
        bool Save(string emailAddress, IMember member);

        /// <summary>
        /// Gets all active users.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMember> GetAllActiveMembers();

        /// <summary>
        /// Gets all active users whose membership details have been edited in the given timeframe.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<IMember> GetActiveMembers(DateTime startDate, DateTime endDate);
    }
}
