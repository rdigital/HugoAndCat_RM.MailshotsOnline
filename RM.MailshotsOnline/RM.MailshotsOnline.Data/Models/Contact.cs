using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Data.Models
{
    [Table("Contacts")]
    public class Contact : IContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ContactId { get; set; }

        public Guid DistributionListId { get; set; }

        [ForeignKey("DistributionListId")]
        public virtual DistributionList DistributionList { get; set; }

        [Column(TypeName = "text")]
        public string SerialisedData { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUtc { get; private set; }

        public PCL.Enums.ContactStatus Status { get; set; }
    }
}
