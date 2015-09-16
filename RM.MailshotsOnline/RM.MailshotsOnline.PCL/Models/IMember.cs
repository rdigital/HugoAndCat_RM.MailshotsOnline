using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    /// <summary>
    /// Wrapper for an Umbraco member
    /// </summary>
    public interface IMember
    {
        /// <summary>
        /// Gets or sets the ID of the member
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the member's username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the member's email address
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the member's title
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the member's first name
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the member's last name
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Indicates whether the user is approved
        /// </summary>
        bool IsApproved { get; set; }

        /// <summary>
        /// Indicates whether the user is locked out
        /// </summary>
        bool IsLockedOut { get; set; }

        /// <summary>
        /// Gets or sets the marketing preferences for Royal Mail
        /// </summary>
        IContactOptions RoyalMailMarketingPreferences { get; set; }

        /// <summary>
        /// Gets or sets the marketing prefernces for third party companies
        /// </summary>
        IContactOptions ThirdPartyMarketingPreferences { get; set; }

        /// <summary>
        /// Gets or sets the member's postcode
        /// </summary>
        string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the member's organisation name
        /// </summary>
        string OrganisationName { get; set; }

        /// <summary>
        /// Gets or sets the member's Job title
        /// </summary>
        string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the member's flat number
        /// </summary>
        string FlatNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's building number
        /// </summary>
        string BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's building name
        /// </summary>
        string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the first line of the member's address
        /// </summary>
        string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the member's address
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the member's city
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Gets or sets the member's country
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Gets or sets the member's work phone number
        /// </summary>
        string WorkPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's mobile number
        /// </summary>
        string MobilePhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the member's password reset token
        /// </summary>
        Guid PasswordResetToken { get; set; }

        /// <summary>
        /// Gets or sets the expiry date of the password reset token
        /// </summary>
        DateTime PasswordResetTokenExpiryDate { get; set; }

        /// <summary>
        /// The random salt
        /// </summary>
        string Salt { get; set; }

        /// <summary>
        /// The salt computed using this member's email address
        /// </summary>
        string EmailSalt { get; set; }

        /// <summary>
        /// Gets a value indicating whether all of the user's required details have been entered (they may have registered through the "light" registration process)
        /// </summary>
        bool AllDetailsEntered { get; }
    }
}
