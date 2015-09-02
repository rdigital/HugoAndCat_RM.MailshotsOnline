using RM.MailshotsOnline.Entities.MemberModels;
using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using umbraco;
using Umbraco.Core;
using HC.RM.Common;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class MemberExtensions
    {
        private static readonly string PrivateKey = ConfigurationManager.AppSettings["PrivateKey"];
        private static readonly RNGCryptoServiceProvider CryptoService = new RNGCryptoServiceProvider();

        /// <summary>
        /// Convert an Umbraco IMember into a H&C IMember
        /// </summary>
        /// <param name="umbracoMember">The umbraco member</param>
        /// <returns>The converted member</returns>
        public static IMember ToMemberEntityModel(this Umbraco.Core.Models.IMember umbracoMember)
        {
            var salt = umbracoMember.GetValue<string>("salt");
            var saltBytes = Convert.FromBase64String(salt);

            var title = Encryption.Decrypt(umbracoMember.GetValue<string>("title"), PrivateKey, saltBytes);
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
                Username = umbracoMember.Username,
                EmailAddress = umbracoMember.Email,
                IsApproved = umbracoMember.IsApproved,
                IsLockedOut = umbracoMember.IsLockedOut,
                Title = title,
                FirstName = Encryption.Decrypt(umbracoMember.GetValue<string>("firstName"), PrivateKey, saltBytes),
                LastName = Encryption.Decrypt(umbracoMember.GetValue<string>("lastName"), PrivateKey, saltBytes),
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
                Postcode = Encryption.Decrypt(umbracoMember.GetValue<string>("postcode"), PrivateKey, saltBytes),
                OrganisationName = Encryption.Decrypt(umbracoMember.GetValue<string>("organisationName"), PrivateKey, saltBytes),
                JobTitle = Encryption.Decrypt(umbracoMember.GetValue<string>("jobTitle"), PrivateKey, saltBytes),
                FlatNumber = Encryption.Decrypt(umbracoMember.GetValue<string>("flatNumber"), PrivateKey, saltBytes),
                BuildingNumber = Encryption.Decrypt(umbracoMember.GetValue<string>("buildingNumber"), PrivateKey, saltBytes),
                BuildingName = Encryption.Decrypt(umbracoMember.GetValue<string>("buildingName"), PrivateKey, saltBytes),
                Address1 = Encryption.Decrypt(umbracoMember.GetValue<string>("address1"), PrivateKey, saltBytes),
                Address2 = Encryption.Decrypt(umbracoMember.GetValue<string>("address2"), PrivateKey, saltBytes),
                City = Encryption.Decrypt(umbracoMember.GetValue<string>("city"), PrivateKey, saltBytes),
                Country = Encryption.Decrypt(umbracoMember.GetValue<string>("country"), PrivateKey, saltBytes),
                WorkPhoneNumber = Encryption.Decrypt(umbracoMember.GetValue<string>("workPhoneNumber"), PrivateKey, saltBytes),
                MobilePhoneNumber = Encryption.Decrypt(umbracoMember.GetValue<string>("mobilePhoneNumber"), PrivateKey, saltBytes),
                PasswordResetToken = umbracoMember.GetValue<Guid>("passwordResetToken"),
                PasswordResetTokenExpiryDate = umbracoMember.GetValue<DateTime>("passwordResetTokenExpiryDate")
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
                member.Salt = GenerateSalt();
            }

            umbracoMember.SetValue("salt", member.Salt);
            umbracoMember.SetValue("title", Encryption.Encrypt(member.Title, PrivateKey, member.Salt));
            umbracoMember.SetValue("firstName", Encryption.Encrypt(member.FirstName, PrivateKey, member.Salt));
            umbracoMember.SetValue("lastName", Encryption.Encrypt(member.LastName, PrivateKey, member.Salt));
            umbracoMember.SetValue("rmPost", member.RoyalMailMarketingPreferences.Post);
            umbracoMember.SetValue("rmEmail", member.RoyalMailMarketingPreferences.Email);
            umbracoMember.SetValue("rmPhone", member.RoyalMailMarketingPreferences.Phone);
            umbracoMember.SetValue("rmSmsAndOther", member.RoyalMailMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("thirdPartyPost", member.ThirdPartyMarketingPreferences.Post);
            umbracoMember.SetValue("thirdPartyEmail", member.ThirdPartyMarketingPreferences.Email);
            umbracoMember.SetValue("thirdPartyPhone", member.ThirdPartyMarketingPreferences.Phone);
            umbracoMember.SetValue("thirdPartySmsAndOther", member.ThirdPartyMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("postcode", Encryption.Encrypt(member.Postcode, PrivateKey, member.Salt));
            umbracoMember.SetValue("organisationName", Encryption.Encrypt(member.OrganisationName, PrivateKey, member.Salt));
            umbracoMember.SetValue("jobTitle", Encryption.Encrypt(member.JobTitle, PrivateKey, member.Salt));
            umbracoMember.SetValue("flatNumber", Encryption.Encrypt(member.FlatNumber, PrivateKey, member.Salt));
            umbracoMember.SetValue("buildingName", Encryption.Encrypt(member.BuildingName, PrivateKey, member.Salt));
            umbracoMember.SetValue("buildingNumber", Encryption.Encrypt(member.BuildingNumber, PrivateKey, member.Salt));
            umbracoMember.SetValue("address1", Encryption.Encrypt(member.Address1, PrivateKey, member.Salt));
            umbracoMember.SetValue("address2", Encryption.Encrypt(member.Address2, PrivateKey, member.Salt));
            umbracoMember.SetValue("city", Encryption.Encrypt(member.City, PrivateKey, member.Salt));
            umbracoMember.SetValue("country", Encryption.Encrypt(member.Country, PrivateKey, member.Salt));
            umbracoMember.SetValue("workPhoneNumber", Encryption.Encrypt(member.WorkPhoneNumber, PrivateKey, member.Salt));
            umbracoMember.SetValue("mobilePhoneNumber", Encryption.Encrypt(member.MobilePhoneNumber, PrivateKey, member.Salt));
            umbracoMember.SetValue("passwordResetToken", member.PasswordResetToken.ToString());
            umbracoMember.SetValue("passwordResetTokenExpiryDate", member.PasswordResetTokenExpiryDate.ToString());
            umbracoMember.Email = member.EmailAddress;
            umbracoMember.Name = umbracoMember.Email;

            return umbracoMember;
        }

        private static string GenerateSalt()
        {
            var salt = new byte[16];
            CryptoService.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }
    }
}
