using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels
{
    [Table("DistributionLists")]
    public class DistributionList : IDistributionList
    {
        private ICollection<Contact> _contacts;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DistributionListId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate
        {
            get { return CreatedUtc; }
        }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public ICollection<Contact> Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }

        #region Explicit Interface Implementations
        ICollection<IContact> IDistributionList.Contacts
        {
            get { return (ICollection<IContact>)_contacts; }
            set { _contacts = (ICollection<Contact>)value; }
        }
        #endregion
    }
}
