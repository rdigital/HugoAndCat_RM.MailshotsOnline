using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco;
using Umbraco.Core;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class MemberExtensions
    {
        public static IMember ToMemberEntityModel(this Umbraco.Core.Models.IMember umbracoMember)
        {
            var title = umbracoMember.GetValue<string>("title");
            var titleValue =
                Umbraco.Core.ApplicationContext.Current.Services.DataTypeService.GetPreValues("Title Dropdown")
                    .FirstOrDefault(x => x.Key.Equals(title));

            if (!string.IsNullOrEmpty(titleValue.Value))
            {
                title = titleValue.Value;
            }

            //TODO Decrypt values
            return new Member()
            {
                Id = umbracoMember.Id,
                EmailAddress = umbracoMember.Email,
                IsApproved = umbracoMember.IsApproved,
                IsLockedOut = umbracoMember.IsLockedOut,
                Title = title,
                FirstName = umbracoMember.GetValue<string>("firstName"),
                LastName = umbracoMember.GetValue<string>("lastName"),
                RoyalMailMarketingPreferences = new ContactOptions()
                {
                    Post = umbracoMember.GetValue<bool>("rmPost"),
                    Email = umbracoMember.GetValue<bool>("rmEmail"),
                    Phone = umbracoMember.GetValue<bool>("rmPhone"),
                    SmsAndOther = umbracoMember.GetValue<bool>("rmSmsAndOther")
                },
                ThirdPartyMarketingPreferences = new ContactOptions()
                {
                    Post = umbracoMember.GetValue<bool>("thirdPartyPost"),
                    Email = umbracoMember.GetValue<bool>("thirdPartyEmail"),
                    Phone = umbracoMember.GetValue<bool>("thirdPartyPhone"),
                    SmsAndOther = umbracoMember.GetValue<bool>("thirdPartySmsAndOther")
                },
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
                MobilePhoneNumber = umbracoMember.GetValue<string>("mobilePhoneNumber"),
                PasswordResetToken = umbracoMember.GetValue<Guid>("passwordResetToken"),
                PasswordResetTokenExpiryDate = umbracoMember.GetValue<DateTime>("passwordResetTokenExpiryDate")
            };
        }

        public static Umbraco.Core.Models.IMember UpdateValues(this Umbraco.Core.Models.IMember umbracoMember, IMember member)
        {
            //TODO: Encryt values
            umbracoMember.SetValue("title", member.Title);
            umbracoMember.SetValue("firstName", member.FirstName);
            umbracoMember.SetValue("lastName", member.LastName);
            umbracoMember.SetValue("rmPost", member.RoyalMailMarketingPreferences.Post);
            umbracoMember.SetValue("rmEmail", member.RoyalMailMarketingPreferences.Email);
            umbracoMember.SetValue("rmPhone", member.RoyalMailMarketingPreferences.Phone);
            umbracoMember.SetValue("rmSmsAndOther", member.RoyalMailMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("thirdPartyPost", member.ThirdPartyMarketingPreferences.Post);
            umbracoMember.SetValue("thirdPartyEmail", member.ThirdPartyMarketingPreferences.Email);
            umbracoMember.SetValue("thirdPartyPhone", member.ThirdPartyMarketingPreferences.Phone);
            umbracoMember.SetValue("thirdPartySmsAndOther", member.ThirdPartyMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("postcode", member.Postcode);
            umbracoMember.SetValue("organisationName", member.OrganisationName);
            umbracoMember.SetValue("jobTitle", member.JobTitle);
            umbracoMember.SetValue("flatNumber", member.FlatNumber);
            umbracoMember.SetValue("buildingName", member.BuildingName);
            umbracoMember.SetValue("buildingNumber", member.BuildingNumber);
            umbracoMember.SetValue("address1", member.Address1);
            umbracoMember.SetValue("address2", member.Address2);
            umbracoMember.SetValue("city", member.City);
            umbracoMember.SetValue("country", member.Country);
            umbracoMember.SetValue("workPhoneNumber", member.WorkPhoneNumber);
            umbracoMember.SetValue("mobilePhoneNumber", member.MobilePhoneNumber);
            umbracoMember.SetValue("passwordResetToken", member.PasswordResetToken.ToString());
            umbracoMember.SetValue("passwordResetTokenExpiryDate", member.PasswordResetTokenExpiryDate.ToString());
            umbracoMember.Email = member.EmailAddress;
            umbracoMember.Name = umbracoMember.Email;

            return umbracoMember;
        }
    }
}
