using RM.MailshotsOnline.PCL.Models;
using System;

namespace RM.MailshotsOnline.Entities.MemberModels
{
    public class Member : IMember
    {
        private ContactOptions _royalMailMarketingPreferences;

        private ContactOptions _thirdPartyMarketingPreferences;

        public int Id { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public ContactOptions RoyalMailMarketingPreferences
        {
            get { return _royalMailMarketingPreferences; }
            set { _royalMailMarketingPreferences = value; }
        }

        public ContactOptions ThirdPartyMarketingPreferences
        {
            get { return _thirdPartyMarketingPreferences; }
            set { _thirdPartyMarketingPreferences = value; }
        }

        public string Postcode { get; set; }

        public string OrganisationName { get; set; }

        public string JobTitle { get; set; }

        public string FlatNumber { get; set; }

        public string BuildingNumber { get; set; }

        public string BuildingName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string WorkPhoneNumber { get; set; }

        public string MobilePhoneNumber { get; set; }

        public Guid PasswordResetToken { get; set; }

        public DateTime PasswordResetTokenExpiryDate { get; set; }

        #region Explicit interface implementation

        IContactOptions IMember.ThirdPartyMarketingPreferences
        {
            get { return _thirdPartyMarketingPreferences; }
            set { _thirdPartyMarketingPreferences = (ContactOptions)value; }
        }

        IContactOptions IMember.RoyalMailMarketingPreferences
        {
            get { return _royalMailMarketingPreferences; }
            set { _royalMailMarketingPreferences = (ContactOptions)value; }
        }

        #endregion
    }
}
