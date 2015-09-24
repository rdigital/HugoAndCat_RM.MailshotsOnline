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
    public class SettingsFromCms : ISettingsFromCms
    {
        /// <summary>
        /// The ID of the settings record
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SettingsId { get; set; }

        /// <summary>
        /// The per-use cost of MSOL
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal MsolPerUseFee { get; set; }

        /// <summary>
        /// The VAT rate
        /// </summary>
        public decimal VatRate { get; set; }

        /// <summary>
        /// The price per record for rented data
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal PricePerRentedDataRecord { get; set; }

        /// <summary>
        /// The price to use the Data Rental service
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal DataRentalServiceFee { get; set; }

        /// <summary>
        /// The Umbraco content ID
        /// </summary>
        [JsonIgnore]
        public int UmbracoContentId { get; set; }

        /// <summary>
        /// Indicates this settings set is the current set
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The date the settings set was created
        /// </summary>
        public DateTime CreatedDate { get { return CreatedUtc; } }

        /// <summary>
        /// The DB generated created date
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonIgnore]
        public DateTime CreatedUtc { get; private set; }

        /// <summary>
        /// The date the settings set was updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the estimated time taken for the moderation process
        /// </summary>
        public int ModerationTimeEstimate { get; set; }

        /// <summary>
        /// Gets or sets the estimated time taken for the printing / despatch time
        /// </summary>
        public int PrintingTimeEstimate { get; set; }

        /// <summary>
        /// Gets or sets a list of public holidays
        /// </summary>
        public string PublicHolidays { get; set; }
    }
}
