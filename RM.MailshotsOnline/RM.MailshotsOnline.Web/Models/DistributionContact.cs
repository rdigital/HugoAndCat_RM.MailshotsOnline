using System;
using System.ComponentModel.DataAnnotations;
using HC.RM.Common.AzureWeb.Attributes;

namespace RM.MailshotsOnline.Web.Models
{
    public class DistributionContact
    {
        public Guid DistributionListId { get; set; }

        public string AddressRef => $"{FlatId}{HouseName}{HouseNumber}{PostCode}".ToLower();

        public string Title { get; set; }
        [RequiredIf("Surname", null, ErrorMessage = "You must have at least a first or surname.")]
        public string FirstName { get; set; }
        [RequiredIf("FirstName", null, ErrorMessage = "You must have at least a first or surname.")]
        public string Surname { get; set; }
        public string FlatId { get; set; }
        public string HouseName { get; set; }
        public string HouseNumber { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        [Required]
        public string PostCode { get; set; }
    }
}
