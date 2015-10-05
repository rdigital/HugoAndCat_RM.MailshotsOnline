using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListMappedFieldsModel<T> where T: IDistributionContact
    {
        public int ValidContactsCount { get; set; }

        public IEnumerable<T> ValidContacts { get; set; }

        public int InvalidContactsCount { get; set; }

        public IEnumerable<T> InvalidContacts { get; set; }

        public int DuplicateContactsCount { get; set; }

        public IEnumerable<T> DuplicateContacts { get; set; }

    }
}
