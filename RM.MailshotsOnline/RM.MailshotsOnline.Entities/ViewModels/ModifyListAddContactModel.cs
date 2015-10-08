using System;
using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListAddContactModel<T> where T: IDistributionContact
    {
        public Guid DistributionListId { get; set; }

        public string ListName { get; set; }

        public IEnumerable<T> Contacts { get; set; }
    }
}
