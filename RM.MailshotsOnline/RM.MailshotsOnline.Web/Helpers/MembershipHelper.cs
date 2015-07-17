using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using Umbraco.Core;

namespace RM.MailshotsOnline.Web.Helpers
{
    public class MembershipHelper
    {
        public static IMember GetCurrentMember(HttpContext httpContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var securityMember = System.Web.Security.Membership.GetUser();

            var umbracoMember = ApplicationContext.Current.Services.MemberService.GetByProviderKey(securityMember.ProviderUserKey);

            // TODO: Encrypt / Decrypt these
            return new Member()
            {
                Id = umbracoMember.Id,
                EmailAddress = umbracoMember.Email,
                IsApproved = umbracoMember.IsApproved,
                IsLockedOut = umbracoMember.IsLockedOut,
                Title = umbracoMember.GetValue<string>("title"),
                FirstName = umbracoMember.GetValue<string>("firstName"),
                LastName = umbracoMember.GetValue<string>("lastName")
            };
        }
    }
}