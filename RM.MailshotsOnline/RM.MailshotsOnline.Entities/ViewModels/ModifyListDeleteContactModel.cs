using System;
using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ModifyListDeleteContactModel
    {
        public Guid DistributionListId { get; set; }

        public IEnumerable<Guid> ContactIds { get; set; }
    }
}
