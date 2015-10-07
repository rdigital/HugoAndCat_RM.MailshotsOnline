using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListSummaryModel<T> : IModifyListSummaryModel<T> where T: IDistributionContact
    {
        public Guid DistributionListId { get; set; }

        [Required]
        public string ListName { get; set; }

        public ListCreate PageModel { get; set; }

        public int ValidContactCount { get; set; }

        public int ValidContactsAdded { get; set; }

        public int TotalContactCount { get; set; }

        public int InvalidContactCount { get; set; }

        public int InvalidContactsAdded { get; set; }

        public int DuplicateContactCount { get; set; }

        public int DuplicateContactsAdded { get; set; }

        public int TotalFoundCount => ValidContactCount + InvalidContactCount + DuplicateContactCount;

        public IEnumerable<T> InvalidContacts { get; set; }

        public IEnumerable<T> DuplicateContacts { get; set; } 
    }
}
