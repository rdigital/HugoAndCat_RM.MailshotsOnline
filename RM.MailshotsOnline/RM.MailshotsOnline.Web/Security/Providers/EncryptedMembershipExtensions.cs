using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace RM.MailshotsOnline.Web.Security.Providers
{
    public static class EncryptedMembershipExtensions
    {
        public static MembershipUser Decrypt(this MembershipUser encryptedUser, MembershipProvider membershipProvider)
        {
            var result = new MembershipUser(
                membershipProvider.Name,
                encryptedUser.UserName, 
                encryptedUser.ProviderUserKey, 
                Decrypt(encryptedUser.Email), 
                Decrypt(encryptedUser.PasswordQuestion), 
                Decrypt(encryptedUser.Comment),
                encryptedUser.IsApproved,
                encryptedUser.IsLockedOut,
                encryptedUser.CreationDate,
                encryptedUser.LastLoginDate,
                encryptedUser.LastActivityDate,
                encryptedUser.LastPasswordChangedDate,
                encryptedUser.LastLockoutDate);

            return result;
        }

        public static string Decrypt(string input)
        {
            //TODO: Actuall decrypt this
            return input;
        }
    }
}
