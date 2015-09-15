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
using HC.RM.Common.PCL;

namespace RM.MailshotsOnline.Data.Extensions
{
    public static class MemberExtensions
    {
        private static readonly string EncryptionKey = Constants.Constants.Encryption.EncryptionKey;
        private static readonly RNGCryptoServiceProvider CryptoService = new RNGCryptoServiceProvider();

        /// <summary>
        /// Convert an Umbraco IMember into a H&C IMember
        /// </summary>
        /// <param name="umbracoMember">The umbraco member</param>
        /// <returns>The converted member</returns>
        public static IMember ToMemberEntityModel(this Umbraco.Core.Models.IMember umbracoMember)
        {
            var salt = umbracoMember.GetValue<string>("salt");
            byte[] saltBytes = null;
            if (!string.IsNullOrEmpty(salt))
            {
                saltBytes = Convert.FromBase64String(salt);
            }

            var emailSalt = umbracoMember.GetValue<string>("emailSalt");
            var emailSaltBytes = Convert.FromBase64String(emailSalt);

            var title = saltBytes == null ? umbracoMember.GetValue<string>("title") : Encryption.Decrypt(umbracoMember.GetValue<string>("title"), PrivateKey, saltBytes);            var titleValue = ApplicationContext.Current.Services.DataTypeService.GetPreValues("Title Dropdown")
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
                EmailAddress = emailSaltBytes == null ? umbracoMember.Email : Encryption.Decrypt(umbracoMember.Email, EncryptionKey, emailSaltBytes,
                IsApproved = umbracoMember.IsApproved,
                IsLockedOut = umbracoMember.IsLockedOut,
                Title = title,
                FirstName = saltBytes == null ? umbracoMember.GetValue<string>("firstName") : Encryption.Decrypt(umbracoMember.GetValue<string>("firstName"), PrivateKey, saltBytes),
                LastName = saltBytes == null ? umbracoMember.GetValue<string>("lastName") : Encryption.Decrypt(umbracoMember.GetValue<string>("lastName"), PrivateKey, saltBytes),
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
                Postcode = saltBytes == null ? umbracoMember.GetValue<string>("postcode") : Encryption.Decrypt(umbracoMember.GetValue<string>("postcode"), PrivateKey, saltBytes),
                OrganisationName = saltBytes == null ? umbracoMember.GetValue<string>("organisationName") : Encryption.Decrypt(umbracoMember.GetValue<string>("organisationName"), PrivateKey, saltBytes),
                JobTitle = saltBytes == null ? umbracoMember.GetValue<string>("jobTitle") : Encryption.Decrypt(umbracoMember.GetValue<string>("jobTitle"), PrivateKey, saltBytes),
                FlatNumber = saltBytes == null ? umbracoMember.GetValue<string>("flatNumber") : Encryption.Decrypt(umbracoMember.GetValue<string>("flatNumber"), PrivateKey, saltBytes),
                BuildingNumber = saltBytes == null ? umbracoMember.GetValue<string>("buildingNumber") : Encryption.Decrypt(umbracoMember.GetValue<string>("buildingNumber"), PrivateKey, saltBytes),
                BuildingName = saltBytes == null ? umbracoMember.GetValue<string>("buildingName") : Encryption.Decrypt(umbracoMember.GetValue<string>("buildingName"), PrivateKey, saltBytes),
                Address1 = saltBytes == null ? umbracoMember.GetValue<string>("address1") : Encryption.Decrypt(umbracoMember.GetValue<string>("address1"), PrivateKey, saltBytes),
                Address2 = saltBytes == null ? umbracoMember.GetValue<string>("address2") : Encryption.Decrypt(umbracoMember.GetValue<string>("address2"), PrivateKey, saltBytes),
                City = saltBytes == null ? umbracoMember.GetValue<string>("city") : Encryption.Decrypt(umbracoMember.GetValue<string>("city"), PrivateKey, saltBytes),
                Country = saltBytes == null ? umbracoMember.GetValue<string>("country") : Encryption.Decrypt(umbracoMember.GetValue<string>("country"), PrivateKey, saltBytes),
                WorkPhoneNumber = saltBytes == null ? umbracoMember.GetValue<string>("workPhoneNumber") : Encryption.Decrypt(umbracoMember.GetValue<string>("workPhoneNumber"), PrivateKey, saltBytes),
                MobilePhoneNumber = saltBytes == null ? umbracoMember.GetValue<string>("mobilePhoneNumber") : Encryption.Decrypt(umbracoMember.GetValue<string>("mobilePhoneNumber"), PrivateKey, saltBytes),
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

            if (member.EmailSalt == null)
            {
                var emailSalt = Encryption.ComputedSalt(member.EmailAddress, member.EmailAddress);
                var emailSaltBytes = Encoding.UTF8.GetBytes(emailSalt);
                member.EmailSalt = Convert.ToBase64String(emailSaltBytes);
            }

            umbracoMember.SetValue("salt", member.Salt);
            umbracoMember.SetValue("emailSalt", member.EmailSalt);
            umbracoMember.SetValue("title", Encryption.Encrypt(member.Title, EncryptionKey, member.Salt));
            umbracoMember.SetValue("firstName", Encryption.Encrypt(member.FirstName, EncryptionKey, member.Salt));
            umbracoMember.SetValue("lastName", Encryption.Encrypt(member.LastName, EncryptionKey, member.Salt));
            umbracoMember.SetValue("rmPost", member.RoyalMailMarketingPreferences.Post);
            umbracoMember.SetValue("rmEmail", member.RoyalMailMarketingPreferences.Email);
            umbracoMember.SetValue("rmPhone", member.RoyalMailMarketingPreferences.Phone);
            umbracoMember.SetValue("rmSmsAndOther", member.RoyalMailMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("thirdPartyPost", member.ThirdPartyMarketingPreferences.Post);
            umbracoMember.SetValue("thirdPartyEmail", member.ThirdPartyMarketingPreferences.Email);
            umbracoMember.SetValue("thirdPartyPhone", member.ThirdPartyMarketingPreferences.Phone);
            umbracoMember.SetValue("thirdPartySmsAndOther", member.ThirdPartyMarketingPreferences.SmsAndOther);
            umbracoMember.SetValue("postcode", Encryption.Encrypt(member.Postcode, EncryptionKey, member.Salt));
            umbracoMember.SetValue("organisationName", Encryption.Encrypt(member.OrganisationName, EncryptionKey, member.Salt));
            umbracoMember.SetValue("jobTitle", Encryption.Encrypt(member.JobTitle, EncryptionKey, member.Salt));
            umbracoMember.SetValue("flatNumber", Encryption.Encrypt(member.FlatNumber, EncryptionKey, member.Salt));
            umbracoMember.SetValue("buildingName", Encryption.Encrypt(member.BuildingName, EncryptionKey, member.Salt));
            umbracoMember.SetValue("buildingNumber", Encryption.Encrypt(member.BuildingNumber, EncryptionKey, member.Salt));
            umbracoMember.SetValue("address1", Encryption.Encrypt(member.Address1, EncryptionKey, member.Salt));
            umbracoMember.SetValue("address2", Encryption.Encrypt(member.Address2, EncryptionKey, member.Salt));
            umbracoMember.SetValue("city", Encryption.Encrypt(member.City, EncryptionKey, member.Salt));
            umbracoMember.SetValue("country", Encryption.Encrypt(member.Country, EncryptionKey, member.Salt));
            umbracoMember.SetValue("workPhoneNumber", Encryption.Encrypt(member.WorkPhoneNumber, EncryptionKey, member.Salt));
            umbracoMember.SetValue("mobilePhoneNumber", Encryption.Encrypt(member.MobilePhoneNumber, EncryptionKey, member.Salt));
            umbracoMember.SetValue("passwordResetToken", member.PasswordResetToken.ToString());
            umbracoMember.SetValue("passwordResetTokenExpiryDate", member.PasswordResetTokenExpiryDate.ToString());
            umbracoMember.Email = Encryption.Encrypt(member.EmailAddress, EncryptionKey, member.EmailSalt);
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
