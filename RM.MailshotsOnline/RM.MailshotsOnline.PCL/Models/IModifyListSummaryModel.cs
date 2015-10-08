using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IModifyListSummaryModel<T> where T : IDistributionContact
    {
        Guid DistributionListId { get; set; }
        string ListName { get; set; }
        int ValidContactCount { get; set; }
        int ValidContactsAdded { get; set; }
        int TotalContactCount { get; set; }
        int InvalidContactCount { get; set; }
        int InvalidContactsAdded { get; set; }
        int DuplicateContactCount { get; set; }
        int DuplicateContactsAdded { get; set; }
        int TotalFoundCount { get; }
        IEnumerable<T> InvalidContacts { get; set; }
        IEnumerable<T> DuplicateContacts { get; set; }
    }
}
