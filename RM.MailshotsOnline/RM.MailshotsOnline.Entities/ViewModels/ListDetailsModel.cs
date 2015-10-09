using System.Collections.Generic;
using RM.MailshotsOnline.PCL.Models;

namespace RM.MailshotsOnline.Entities.ViewModels
{
    public class ListDetailsModel<T> where T : IDistributionContact
    {
        public IDistributionList List { get; set; }

        public List<T> Contacts { get; set; }
    }
}
