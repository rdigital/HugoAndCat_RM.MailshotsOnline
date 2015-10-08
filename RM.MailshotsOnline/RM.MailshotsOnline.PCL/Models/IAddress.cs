using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IAddress
    {
        /// <summary>
        /// Gets or sets the recipients title or salutation
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets the flat number
        /// </summary>
        string FlatNumber { get; set; }

        /// <summary>
        /// Gets or sets the building number
        /// </summary>
        string BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the first line of the address
        /// </summary>
        string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the address
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Gets or sets the county
        /// </summary>
        string County { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Gets or sets the postcode
        /// </summary>
        string Postcode { get; set; }
    }
}
