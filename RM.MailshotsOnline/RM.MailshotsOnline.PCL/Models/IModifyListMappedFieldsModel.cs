using System.Collections.Generic;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IModifyListMappedFieldsModel<T> where T : IDistributionContact
    {
        int ValidContactsCount { get; set; }
        IEnumerable<T> ValidContacts { get; set; }
        int InvalidContactsCount { get; set; }
        IEnumerable<T> InvalidContacts { get; set; }
        int DuplicateContactsCount { get; set; }
        IEnumerable<T> DuplicateContacts { get; set; }
    }
}
