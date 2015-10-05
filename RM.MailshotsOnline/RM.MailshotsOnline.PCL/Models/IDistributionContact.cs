using System;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IDistributionContact
    {
        Guid ContactId { get; set; }
        string AddressRef { get; }
        string Title { get; set; }
        string FirstName { get; set; }
        string Surname { get; set; }
        string FlatId { get; set; }
        string HouseName { get; set; }
        string HouseNumber { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        string Address4 { get; set; }
        string PostCode { get; set; }
        string ToString(string format);
    }
}
