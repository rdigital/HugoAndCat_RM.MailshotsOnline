﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IMember
    {
        int Id { get; set; }

        string EmailAddress { get; set; }

        string Title { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        bool IsApproved { get; set; }

        bool IsLockedOut { get; set; }

        bool CanWeContactByPost { get; set; }

        bool CanWeContactByEmail { get; set; }

        bool CanWeContactByPhone { get; set; }

        bool CanWeContactBySmsAndOther { get; set; }

        bool CanThirdPatiesContactByPost { get; set; }

        bool CanThirdPatiesContactByEmail { get; set; }

        bool CanThirdPatiesContactByPhone { get; set; }

        bool CanThirdPatiesContactBySmsAndOther { get; set; }

        string Postcode { get; set; }

        string OrganisationName { get; set; }

        string JobTitle { get; set; }

        string FlatNumber { get; set; }

        string BuildingNumber { get; set; }

        string BuildingName { get; set; }

        string Address1 { get; set; }

        string Address2 { get; set; }

        string City { get; set; }

        string Country { get; set; }

        string WorkPhoneNumber { get; set; }

        string MobilePhoneNumber { get; set; }

        Guid PasswordResetToken { get; set; }

        DateTime PasswordResetTokenExpiryDate { get; set; }

    }
}
