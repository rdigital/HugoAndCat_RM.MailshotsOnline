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
    [Table("PostalOptions")]
    public class PostalOption : IPostalOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PostalOptionId { get; set; }

        public int FormatId { get; set; }

        public string Name { get; set; }

        [MaxLength(3)]
        public string Currency { get; set; }

        [Column(TypeName = "Money")]
        public decimal PricePerUnit { get; set; }

        [Column(TypeName = "Money")]
        public decimal Tax { get; set; }

        [MaxLength(1)]
        public string TaxCode { get; set; }
    }
}
