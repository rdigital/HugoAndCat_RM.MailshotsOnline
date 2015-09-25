using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels
{
    [ComplexType]
    public class Address : IAddress
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the flat number
        /// </summary>
        public string FlatNumber { get; set; }

        /// <summary>
        /// Gets or sets the building number
        /// </summary>
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the first line of the address
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the address
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the postcode
        /// </summary>
        public string Postcode { get; set; }
    }
}
