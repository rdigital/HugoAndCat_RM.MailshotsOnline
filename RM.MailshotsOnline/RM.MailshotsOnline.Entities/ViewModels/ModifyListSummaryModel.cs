using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RM.MailshotsOnline.Entities.PageModels;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListSummaryModel
    {
        public Guid DistributionListId { get; set; }

        [Required]
        public string ListName { get; set; }

        public ListCreate PageModel { get; set; }

        public int ValidContactCount { get; set; }

        public int TotalContactCount { get; set; }

        public int InvalidContactCount { get; set; }

        public int DuplicateContactCount { get; set; }

        public int TotalFoundCount => ValidContactCount + InvalidContactCount + DuplicateContactCount;

        public IEnumerable<IDistributionContact> InvalidContacts { get; set; }

        public IEnumerable<IDistributionContact> DuplicateContacts { get; set; } 
    }
}
