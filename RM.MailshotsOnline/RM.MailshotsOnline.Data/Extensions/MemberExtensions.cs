using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using umbraco;
using Umbraco.Core;
using HC.RM.Common;
using HC.RM.Common.PCL;
using RM.MailshotsOnline.Data.Services;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class MemberExtensions
    {
        private static readonly CryptographicService CryptographicService = new CryptographicService();

        /// <summary>
        /// Convert an Umbraco IMember into a H&C IMember
        /// </summary>
        /// <param name="umbracoMember">The umbraco member</param>
        /// <returns>The converted member</returns>
        public static IMember ToMemberEntityModel(this Umbraco.Core.Models.IMember umbracoMember)
        {
            var salt = umbracoMember.GetValue<string>("salt");
            var saltBytes = Convert.FromBase64String(salt);
            var emailSalt = umbracoMember.GetValue<string>("emailSalt");

            var title = CryptographicService.Decrypt(umbracoMember.GetValue<string>("title"), saltBytes);
            var titleValue = ApplicationContext.Current.Services.DataTypeService.GetPreValues("Title Dropdown")
                .FirstOrDefault(x => x.Key.Equals(title));

            if (!string.IsNullOrEmpty(titleValue.Value))
            {
                title = titleValue.Value;
            }

            return new Member()
            {
                Id = umbracoMember.Id,
                Salt = salt,
                EmailSalt = emailSalt,
                Username = umbracoMember.Username,
                EmailAddress = CryptographicService.Decrypt(umbracoMember.Email, emailSalt),
                IsApproved = umbracoMember.IsApproved,
                IsLockedOut = umbracoMember.IsLockedOut,
                Title = title,
                FirstName = CryptographicService.Decrypt(umbracoMember.GetValue<string>("firstName"), saltBytes),
                LastName = CryptographicService.Decrypt(umbracoMember.GetValue<string>("lastName"), saltBytes),
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
                Postcode = CryptographicService.Decrypt(umbracoMember.GetValue<string>("postcode"), saltBytes),
                OrganisationName = CryptographicService.Decrypt(umbracoMember.GetValue<string>("organisationName"), saltBytes),
                JobTitle = CryptographicService.Decrypt(umbracoMember.GetValue<string>("jobTitle"), saltBytes),
                FlatNumber = CryptographicService.Decrypt(umbracoMember.GetValue<string>("flatNumber"), saltBytes),
                BuildingNumber = CryptographicService.Decrypt(umbracoMember.GetValue<string>("buildingNumber"), saltBytes),
                BuildingName = CryptographicService.Decrypt(umbracoMember.GetValue<string>("buildingName"), saltBytes),
                Address1 = CryptographicService.Decrypt(umbracoMember.GetValue<string>("address1"), saltBytes),
                Address2 = CryptographicService.Decrypt(umbracoMember.GetValue<string>("address2"), saltBytes),
                City = CryptographicService.Decrypt(umbracoMember.GetValue<string>("city"), saltBytes),
                Country = CryptographicService.Decrypt(umbracoMember.GetValue<string>("country"), saltBytes),
                WorkPhoneNumber = CryptographicService.Decrypt(umbracoMember.GetValue<string>("workPhoneNumber"), saltBytes),
                MobilePhoneNumber = CryptographicService.Decrypt(umbracoMember.GetValue<string>("mobilePhoneNumber"), saltBytes),
                PasswordResetToken = umbracoMember.GetValue<Guid>("passwordResetToken"),
                PasswordResetTokenExpiryDate = umbracoMember.GetValue<DateTime>("passwordResetTokenExpiryDate"),
                Updated = umbracoMember.UpdateDate
            };
        }

        /// <summary>
        /// Sets the values of a H&C IMember into the given umbraco member.
        /// </summary>
        /// <param name="umbracoMember">The umbraco member</param>
        /// <param name="member">The H&C IMember</param>
        /// <returns></returns>
        public static Umbraco.Core.Models.IMember UpdateValues(this Umbraco.Core.Models.IMember umbracoMember, IMember member)
        {
            if (member.Salt == null)
            {
                member.Salt = CryptographicService.GenerateRandomSaltB64();
            }

            if (member.EmailSalt == null)
            {
                member.EmailSalt = CryptographicService.GenerateEmailSalt(member.EmailAddress);
            }

            umbracoMember.SetValue("salt", member.Salt);
            umbracoMember.SetValue("emailSalt", member.EmailSalt);
            umbracoMember.SetValue("title", CryptographicService.Encrypt(member.Title, member.Salt));
            umbracoMember.SetValue("firstName", CryptographicService.Encrypt(member.FirstName, member.Salt));
            umbracoMember.SetValue("lastName", CryptographicService.Encrypt(member.LastName, member.Salt));
            umbracoMember.SetValue("rmPost", member.RoyalMailMarketingPreferences.Post);
            umbracoMember.SetValue("rmEmail", member.RoyalMailMarketingPreferences.Email);
            umbracoMember.SetValue("rmPhone", member.RoyalMailMarketingPreferences.Phone);
            umbracoMember.SetValue("rmSmsAndOther", member.RoyalMailMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("thirdPartyPost", member.ThirdPartyMarketingPreferences.Post);
            umbracoMember.SetValue("thirdPartyEmail", member.ThirdPartyMarketingPreferences.Email);
            umbracoMember.SetValue("thirdPartyPhone", member.ThirdPartyMarketingPreferences.Phone);
            umbracoMember.SetValue("thirdPartySmsAndOther", member.ThirdPartyMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("postcode", CryptographicService.Encrypt(member.Postcode, member.Salt));
            umbracoMember.SetValue("organisationName", CryptographicService.Encrypt(member.OrganisationName, member.Salt));
            umbracoMember.SetValue("jobTitle", CryptographicService.Encrypt(member.JobTitle, member.Salt));
            umbracoMember.SetValue("flatNumber", CryptographicService.Encrypt(member.FlatNumber, member.Salt));
            umbracoMember.SetValue("buildingName", CryptographicService.Encrypt(member.BuildingName, member.Salt));
            umbracoMember.SetValue("buildingNumber", CryptographicService.Encrypt(member.BuildingNumber, member.Salt));
            umbracoMember.SetValue("address1", CryptographicService.Encrypt(member.Address1, member.Salt));
            umbracoMember.SetValue("address2", CryptographicService.Encrypt(member.Address2, member.Salt));
            umbracoMember.SetValue("city", CryptographicService.Encrypt(member.City, member.Salt));
            umbracoMember.SetValue("country", CryptographicService.Encrypt(member.Country, member.Salt));
            umbracoMember.SetValue("workPhoneNumber", CryptographicService.Encrypt(member.WorkPhoneNumber, member.Salt));
            umbracoMember.SetValue("mobilePhoneNumber", CryptographicService.Encrypt(member.MobilePhoneNumber, member.Salt));
            umbracoMember.SetValue("passwordResetToken", member.PasswordResetToken.ToString());
            umbracoMember.SetValue("passwordResetTokenExpiryDate", member.PasswordResetTokenExpiryDate.ToString());
            umbracoMember.Email = CryptographicService.Encrypt(member.EmailAddress, member.EmailSalt);
            umbracoMember.Name = umbracoMember.Email;

            return umbracoMember;
        }

        
    }
}
