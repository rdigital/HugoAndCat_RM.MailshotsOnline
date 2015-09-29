using Newtonsoft.Json;
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
    public class Product : IProduct
    {
        /// <summary>
        /// Gets or sets the product stock code
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(128)]
        public string ProductSku { get; set; }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        [MaxLength(1024)]
        public string Name { get; set; }

        /// <summary>
        /// The category for the product
        /// </summary>
        [MaxLength(1024)]
        public string Category { get; set; }

        /// <summary>
        /// The date the product was created
        /// </summary>
        public DateTime CreatedDate { get { return CreatedUtc; } }

        /// <summary>
        /// The DB generated created date
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonIgnore]
        public DateTime CreatedUtc { get; private set; }

        /// <summary>
        /// Gets or sets the updated date
        /// </summary>
        public DateTime UpdatedDate { get; set; }
    }
}
