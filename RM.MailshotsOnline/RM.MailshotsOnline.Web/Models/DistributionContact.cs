using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using HC.RM.Common.AzureWeb.Attributes;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Web.Models
{
    [DataContract]
    public class DistributionContact : IDistributionContact, IEquatable<DistributionContact>
    {
        public bool Equals(DistributionContact other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || ContactId.Equals(other.ContactId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((DistributionContact)obj);
        }

        public override int GetHashCode()
        {
            return ContactId.GetHashCode();
        }

        [DataMember]
        public Guid ContactId { get; set; }

        public string AddressRef => $"{FlatId}{HouseName}{HouseNumber}{PostCode}".ToLower();

        [DataMember]
        public string Title { get; set; }

        [RequiredIf("Surname", "", ErrorMessage = "You must have at least a first or surname.")]
        [DataMember]
        public string FirstName { get; set; }

        [RequiredIf("FirstName", "", ErrorMessage = "You must have at least a first or surname.")]
        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string FlatId { get; set; }

        [DataMember]
        public string HouseName { get; set; }

        [DataMember]
        public string HouseNumber { get; set; }

        [Required]
        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string Address3 { get; set; }

        [DataMember]
        public string Address4 { get; set; }

        [Required]
        [DataMember]
        public string PostCode { get; set; }

        public string ToString(string format)
        {
            switch (format.ToLower())
            {
                case "n":
                    return $"{Title} {FirstName} {Surname}".Trim().Replace("  ", " ");
                case "a":
                    return $"{FlatId} {HouseName} {HouseNumber} {Address1} {Address2}".Trim().Replace("  ", " ");
            }

            return ToString();
        }
    }
}
