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
        public Address()
        {

        }

        internal Address(PplShippingAddress address)
            : this((PplAddress)address)
        {
            this.Id = address.id;
            this.RecipientName = address.recipient_name;
            //this.Type = Enum.Parse(typeof(AddressType), address.)
        }

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

        public string Id { get; set; }

        public string RecipientName { get; set; }

        //public AddressType Type { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string CountryCode { get; set; }

        public string PostalCode { get; set; }

        public string State { get; set; }

        public string Phone { get; set; }

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
