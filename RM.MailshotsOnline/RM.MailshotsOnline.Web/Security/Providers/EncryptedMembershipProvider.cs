using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace RM.MailshotsOnline.Web.Security.Providers
{
    public class EncryptedMembershipProvider : Umbraco.Web.Security.Providers.MembersMembershipProvider
    {
        public override string Name
        {
            get
            {
                return "EncryptedMembershipProvider";
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var encryptedMember = base.GetUser(providerUserKey, userIsOnline);

            var decryptedMember = encryptedMember.Decrypt(this);

            return decryptedMember;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var encryptedMember = base.GetUser(username, userIsOnline);

            var decryptedMember = encryptedMember.Decrypt(this);

            return decryptedMember;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var encryptedCollection = base.GetAllUsers(pageIndex, pageSize, out totalRecords);

            var result = new MembershipUserCollection();
            foreach (MembershipUser encryptedMember in encryptedCollection)
            {
                result.Add(encryptedMember.Decrypt(this));
            }

            return result;
        }
    }
}
