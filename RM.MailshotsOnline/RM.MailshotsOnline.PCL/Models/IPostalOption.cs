using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IPostalOption
    {
        /// <summary>
        /// Gets or sets the ID of the postal option
        /// </summary>
        Guid PostalOptionId { get; set; }

        /// <summary>
        /// Gets or sets the Umbraco ID
        /// </summary>
        int UmbracoId { get; set; }

        /// <summary>
        /// Gets or sets the name of the option
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency for the option
        /// </summary>
        string Currency { get; set; }

        /// <summary>
        /// Gets or sets the price per unit
        /// </summary>
        decimal PricePerUnit { get; set; }

        /// <summary>
        /// Gets or sets the tax per unit
        /// </summary>
        decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets the tax code
        /// </summary>
        string TaxCode { get; set; }
    }
}
