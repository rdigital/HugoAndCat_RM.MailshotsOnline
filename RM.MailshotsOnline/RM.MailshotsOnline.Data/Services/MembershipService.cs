using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

            // TODO: Encrypt / Decrypt these

            return new Member()
            {
                Id = umbracoMember.Id,
                EmailAddress = umbracoMember.Email,
                IsApproved = umbracoMember.IsApproved,
                IsLockedOut = umbracoMember.IsLockedOut,
                Title = umbracoMember.GetValue<string>("title"),
                FirstName = umbracoMember.GetValue<string>("firstName"),
                LastName = umbracoMember.GetValue<string>("lastName"),
                CanWeContactByPost = umbracoMember.GetValue<bool>("rmPost"),
                CanWeContactByEmail = umbracoMember.GetValue<bool>("rmEmail"),
                CanWeContactByPhone = umbracoMember.GetValue<bool>("rmPhone"),
                CanWeContactBySmsAndOther = umbracoMember.GetValue<bool>("rmSmsAndOther"),
                CanThirdPatiesContactByPost = umbracoMember.GetValue<bool>("thirdPartyPost"),
                CanThirdPatiesContactByEmail = umbracoMember.GetValue<bool>("thirdPartyEmail"),
                CanThirdPatiesContactByPhone = umbracoMember.GetValue<bool>("thirdPartyPhone"),
                CanThirdPatiesContactBySmsAndOther = umbracoMember.GetValue<bool>("thirdPartySmsAndOther"),
                Postcode = umbracoMember.GetValue<string>("postcode"),
                OrganisationName = umbracoMember.GetValue<string>("organisationName"),
                JobTitle = umbracoMember.GetValue<string>("jobTitle"),
                FlatNumber = umbracoMember.GetValue<string>("flatNumber"),
                BuildingNumber = umbracoMember.GetValue<string>("buildingNumber"),
                BuildingName = umbracoMember.GetValue<string>("buildingName"),
                Address1 = umbracoMember.GetValue<string>("address1"),
                Address2 = umbracoMember.GetValue<string>("address2"),
                City = umbracoMember.GetValue<string>("city"),
                Country = umbracoMember.GetValue<string>("country"),
                WorkPhoneNumber = umbracoMember.GetValue<string>("workPhoneNumber"),
                MobilePhoneNumber = umbracoMember.GetValue<string>("mobilePhoneNumber")
            };
        }

        public IMember CreateMember(IMember member, string password)
        {
            var membershipService = Umbraco.Core.ApplicationContext.Current.Services.MemberService;

            if (membershipService.Exists(member.EmailAddress))
            {
                return null;
            }

            //TODO: Encrypt data!

            var umbracoMember = membershipService.CreateMemberWithIdentity(member.EmailAddress, member.EmailAddress,
                member.EmailAddress, "Member");

            umbracoMember.SetValue("title", member.Title);
            umbracoMember.SetValue("firstName", member.FirstName);
            umbracoMember.SetValue("lastName", member.LastName);
            umbracoMember.SetValue("rmPost", member.CanWeContactByPost);
            umbracoMember.SetValue("rmEmail", member.CanWeContactByEmail);
            umbracoMember.SetValue("rmPhone", member.CanWeContactByPhone);
            umbracoMember.SetValue("rmSmsAndOther", member.CanWeContactBySmsAndOther);
            umbracoMember.SetValue("thirdPartyPost", member.CanThirdPatiesContactByPost);
            umbracoMember.SetValue("thirdPartyEmail", member.CanThirdPatiesContactByEmail);
            umbracoMember.SetValue("thirdPartyPhone", member.CanThirdPatiesContactByPhone);
            umbracoMember.SetValue("thirdPartySmsAndOther", member.CanThirdPatiesContactBySmsAndOther);
            umbracoMember.SetValue("postcode", member.Postcode);
            umbracoMember.SetValue("organisationName", member.OrganisationName);
            umbracoMember.SetValue("jobTitle", member.JobTitle);
            umbracoMember.SetValue("flatNumber", member.FlatNumber);
            umbracoMember.SetValue("buildingNumber", member.BuildingName);
            umbracoMember.SetValue("address1", member.Address1);
            umbracoMember.SetValue("address2", member.Address2);
            umbracoMember.SetValue("city", member.City);
            umbracoMember.SetValue("country", member.Country);
            umbracoMember.SetValue("workPhoneNumber", member.WorkPhoneNumber);
            umbracoMember.SetValue("mobilePhoneNumber", member.MobilePhoneNumber);

            Umbraco.Core.ApplicationContext.Current.Services.MemberService.Save(umbracoMember);
            Umbraco.Core.ApplicationContext.Current.Services.MemberService.SavePassword(umbracoMember, password);

            member.Id = umbracoMember.Id;

            return member;
        }
    }
}
