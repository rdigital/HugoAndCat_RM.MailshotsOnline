using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface ISettingsFromCms
    {
        /// <summary>
        /// The ID of the settings record
        /// </summary>
        Guid SettingsId { get; set; }

        /// <summary>
        /// The VAT rate
        /// </summary>
        decimal VatRate { get; set; }

        /// <summary>
        /// The per-use cost of MSOL
        /// </summary>
        decimal MsolPerUseFee { get; set; }

        /// <summary>
        /// The price per record for rented data
        /// </summary>
        decimal PricePerRentedDataRecord { get; set; }

        /// <summary>
        /// The price to use the Data Rental service
        /// </summary>
        decimal DataRentalServiceFee { get; set; }

        /// <summary>
        /// The Umbraco content ID
        /// </summary>
        int UmbracoContentId { get; set; }

        /// <summary>
        /// Indicates this settings set is the current set
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// The date the settings set was created
        /// </summary>
        DateTime CreatedDate { get; }

        /// <summary>
        /// The date the settings set was updated
        /// </summary>
        DateTime UpdatedDate { get; set; }
    }
}
