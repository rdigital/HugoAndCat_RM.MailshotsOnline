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
    [ComplexType]
    public class Address : IAddress
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        [MaxLength(256)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        [MaxLength(256)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the flat number
        /// </summary>
        [MaxLength(64)]
        public string FlatNumber { get; set; }

        /// <summary>
        /// Gets or sets the building number
        /// </summary>
        [MaxLength(64)]
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the building name
        /// </summary>
        [MaxLength(256)]
        public string BuildingName { get; set; }

        /// <summary>
        /// Gets or sets the first line of the address
        /// </summary>
        [MaxLength(256)]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the address
        /// </summary>
        [MaxLength(256)]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        [MaxLength(256)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        [MaxLength(256)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the postcode
        /// </summary>
        [MaxLength(128)]
        public string Postcode { get; set; }
    }
}
