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
    [Table("PostalOptions")]
    public class PostalOption : IPostalOption
    {
        /// <summary>
        /// Gets or sets the ID of the postal option
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PostalOptionId { get; set; }

        /// <summary>
        /// Gets or sets the Umbraco ID
        /// </summary>
        public int UmbracoId { get; set; }

        /// <summary>
        /// Gets or sets the name of the option
        /// </summary>
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency for the option
        /// </summary>
        [MaxLength(3)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the price per unit
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal PricePerUnit { get; set; }

        /// <summary>
        /// Gets or sets the tax per unit
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets the tax code
        /// </summary>
        [MaxLength(1)]
        public string TaxCode { get; set; }

        /// <summary>
        /// Gets or sets the delivery time if using this option
        /// </summary>
        public int DeliveryTime { get; set; }
    }
}
