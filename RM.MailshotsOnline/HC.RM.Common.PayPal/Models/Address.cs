using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplShippingAddress = PayPal.Api.ShippingAddress;
using PplAddress = PayPal.Api.Address;

namespace HC.RM.Common.PayPal.Models
{
    public class Address
    {
        /// <summary>
        /// Creates an Address object
        /// </summary>
        /// <param name="line1">Line 1 of the address</param>
        /// <param name="city">The City</param>
        /// <param name="countryCode">Two-letter Country Code</param>
        /// <param name="postalCode">The Postal Code</param>
        public Address(string line1, string city, string countryCode, string postalCode)
        {
            this.Line1 = line1;
            this.City = city;
            this.CountryCode = countryCode;
            this.PostalCode = postalCode;
        }

        /// <summary>
        /// Creates an Address object from a PayPal Shipping Address
        /// </summary>
        /// <param name="address">The PayPal Shipping Address</param>
        internal Address(PplShippingAddress address)
            : this((PplAddress)address)
        {
            this.Id = address.id;
            this.RecipientName = address.recipient_name;
        }

        /// <summary>
        /// Creates an Address object from a PayPal Address
        /// </summary>
        /// <param name="address">The PayPal Address</param>
        internal Address(PplAddress address)
        {
            this.Line1 = address.line1;
            this.Line2 = address.line2;
            this.City = address.city;
            this.CountryCode = address.country_code;
            this.PostalCode = address.postal_code;
            this.State = address.state;
            this.Phone = address.phone;
        }

        /// <summary>
        /// The ID of the address (Only applies to Shipping addresses)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Recipient name (Only applies to Shipping Addresses)
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// Line 1 of the address (required)
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Line 2 of the address
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// The City of the address (required)
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Two-letter country code (required)
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Post code (required)
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// State (2-letter code for US states)
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Converts the Address to a PayPal Address
        /// </summary>
        /// <returns>PayPal Address</returns>
        internal PplAddress ToPaypalAddress()
        {
            var result = new PplAddress();

            result.city = this.City;
            result.country_code = this.CountryCode;
            result.line1 = this.Line1;
            result.line2 = this.Line2;
            result.phone = this.Phone;
            result.postal_code = this.PostalCode;
            result.state = this.State;

            return result;
        }

        /// <summary>
        /// Converts the Address to a PayPal Shipping address
        /// </summary>
        /// <returns></returns>
        internal PplShippingAddress ToPaypalShippingAddress()
        {
            var result = (PplShippingAddress)this.ToPaypalAddress();

            result.recipient_name = this.RecipientName;
            result.id = this.Id;

            return result;
        }
    }

    //public enum AddressType
    //{
    //    residential,
    //    business,
    //    mailbox
    //}
}
