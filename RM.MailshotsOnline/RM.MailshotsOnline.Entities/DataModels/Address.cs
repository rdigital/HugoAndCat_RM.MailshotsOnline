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

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
            {
                sb.AppendFormat("{0} {1}", FirstName, LastName);
            }
            else if (!string.IsNullOrEmpty(FirstName))
            {
                sb.AppendFormat(FirstName);
            }
            else if (!string.IsNullOrEmpty(LastName))
            {
                sb.AppendFormat(LastName);
            }

            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(FlatNumber))
            {
                sb.AppendFormat("{0} ", FlatNumber);
            }
            if (!string.IsNullOrEmpty(BuildingNumber))
            {
                sb.AppendFormat("{0} ", BuildingNumber);
            }
            if (!string.IsNullOrEmpty(BuildingName))
            {
                sb.AppendFormat("{0} ", BuildingName);
            }
            sb.AppendLine(Address1);

            if (!string.IsNullOrEmpty(Address2))
            {
                sb.AppendLine(Address2);
            }

            if (!string.IsNullOrEmpty(City))
            {
                sb.AppendLine(City);
            }

            if (!string.IsNullOrEmpty(Postcode))
            {
                sb.AppendLine(Postcode);
            }

            if (!string.IsNullOrEmpty(Country))
            {
                sb.AppendLine(Country);
            }

            return sb.ToString();
        }
    }
}
