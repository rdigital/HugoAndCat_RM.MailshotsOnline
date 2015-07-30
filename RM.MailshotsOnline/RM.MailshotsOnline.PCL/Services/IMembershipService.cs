using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface IMembershipService
    {
        IMember GetCurrentMember();

        IMember CreateMember(IMember member, string password);

        Guid? RequestPasswordReset(string email);

        IMember GetMemberByPasswordResetToken(string token);

        void SetNewPassword(IMember member, string password);
    }
}
