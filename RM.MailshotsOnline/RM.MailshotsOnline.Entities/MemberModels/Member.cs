using RM.MailshotsOnline.PCL.Models;
using System;

namespace RM.MailshotsOnline.Entities.MemberModels
{
    /// <summary>
    /// Wrapper for an Umbraco member
    /// </summary>
    public class Member : IMember
    {
        /// <summary>
        /// The concrete model of the RM marketing preferences
        /// </summary>
        private ContactOptions _royalMailMarketingPreferences;

        /// <summary>
        /// The concrete model of the third party company marketing preferences
        /// </summary>
        private ContactOptions _thirdPartyMarketingPreferences;

        /// <summary>
        /// Gets or sets the ID of the member
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the member's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the member's email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the member's title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the member's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the member's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Indicates whether the user is approved
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Indicates whether the user is locked out
        /// </summary>
        public bool IsLockedOut { get; set; }

        /// <summary>
        /// Gets or sets the marketing preferences for Royal Mail
        /// </summary>
        public ContactOptions RoyalMailMarketingPreferences
        {
            get { return _royalMailMarketingPreferences; }
            set { _royalMailMarketingPreferences = value; }
        }

        /// <summary>
        /// Gets or sets the marketing prefernces for third party companies
        /// </summary>
        public ContactOptions ThirdPartyMarketingPreferences
        {
            get { return _thirdPartyMarketingPreferences; }
            set { _thirdPartyMarketingPreferences = value; }
        }

        /// <summary>
        /// Gets or sets the member's postcode
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the member's organisation name
        /// </summary>
        public string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the member's Job title
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the member's flat number
        /// </summary>
        public string FlatNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's building number
        /// </summary>
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's building name
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the first line of the member's address
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the member's address
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the member's city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the member's country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the member's work phone number
        /// </summary>
        public string WorkPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's mobile number
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's password reset token
        /// </summary>
        public Guid PasswordResetToken { get; set; }

        /// <summary>
        /// Gets or sets the expiry date of the password reset token
        /// </summary>
        public DateTime PasswordResetTokenExpiryDate { get; set; }
        
        /// <summary>
        /// Random salt
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Salt computed usin this membe's email address
        /// </summary>
        public string EmailSalt { get; set; }

        /// <summary>
        /// Gets a value indicating whether all of the user's required details have been entered (they may have registered through the "light" registration process)
        /// </summary>
        public bool AllDetailsEntered
        {
            get
            {
                return !string.IsNullOrEmpty(EmailAddress)
                    && !string.IsNullOrEmpty(Title)
                    && !string.IsNullOrEmpty(FirstName)
                    && !string.IsNullOrEmpty(LastName)
                    && !string.IsNullOrEmpty(Postcode)
                    && !string.IsNullOrEmpty(OrganisationName)
                    && !string.IsNullOrEmpty(JobTitle)
                    && !string.IsNullOrEmpty(Address1)
                    && !string.IsNullOrEmpty(City)
                    && !string.IsNullOrEmpty(Country)
                    && !string.IsNullOrEmpty(WorkPhoneNumber);
            }
        }

        /// <summary>
        /// Last updated date
        /// </summary>
        public DateTime Updated { get; set; }

        #region Explicit interface implementation

        /// <summary>
        /// Gets or sets the marketing preferences for Royal Mail
        /// </summary>
        IContactOptions IMember.ThirdPartyMarketingPreferences
        {
            get { return _thirdPartyMarketingPreferences; }
            set { _thirdPartyMarketingPreferences = (ContactOptions)value; }
        }

        /// <summary>
        /// Gets or sets the marketing preferences for Royal Mail
        /// </summary>
        IContactOptions IMember.RoyalMailMarketingPreferences
        {
            get { return _royalMailMarketingPreferences; }
            set { _royalMailMarketingPreferences = (ContactOptions)value; }
        }

        #endregion
    }
}
