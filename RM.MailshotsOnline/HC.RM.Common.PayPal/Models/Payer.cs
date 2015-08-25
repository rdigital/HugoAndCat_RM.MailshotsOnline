using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplPayer = PayPal.Api.Payer;
using PplPayerInfo = PayPal.Api.PayerInfo;
using PplShippingAddress = PayPal.Api.ShippingAddress;
using PplAddress = PayPal.Api.Address;

namespace HC.RM.Common.PayPal.Models
{
    public class Payer
    {
        public Payer()
        {

        }

        internal Payer(PplPayer payer)
        {
            this.Id = payer.payer_info.payer_id;
            this.PaymentMethod = (PayerPaymentMethod)Enum.Parse(typeof(PayerPaymentMethod), payer.payment_method);
            this.Email = payer.payer_info.email;
            this.Salutation = payer.payer_info.salutation;
            this.FirstName = payer.payer_info.first_name;
            this.MiddleName = payer.payer_info.middle_name;
            this.LastName = payer.payer_info.last_name;
            this.Suffix = payer.payer_info.suffix;
            this.Phone = payer.payer_info.phone;
            this.CountryCode = payer.payer_info.country_code;
            this.ShippingAddress = payer.payer_info.shipping_address != null ? new Address(payer.payer_info.shipping_address) : null;
            this.BillingAddress = payer.payer_info.billing_address != null ? new Address(payer.payer_info.billing_address) : null;
        }

        public string Id { get; set; }

        public PayerPaymentMethod PaymentMethod { get; set; }

        public string Email { get; set; }

        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public string Phone { get; set; }

        public string CountryCode { get; set; }

        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }

        public PplPayer ToPaypalPayer()
        {
            var result = new PplPayer();

            result.payer_info = new PplPayerInfo();
            result.payer_info.country_code = this.CountryCode;
            result.payer_info.email = this.Email;
            result.payer_info.first_name = this.FirstName;
            result.payer_info.last_name = this.LastName;
            result.payer_info.middle_name = this.MiddleName;
            result.payer_info.payer_id = this.Id;
            result.payer_info.phone = this.Phone;
            result.payer_info.salutation = this.Salutation;
            result.payer_info.suffix = this.Suffix;
            result.payer_info.billing_address = this.BillingAddress != null ? this.BillingAddress.ToPaypalAddress() : null;
            result.payer_info.shipping_address = this.ShippingAddress != null ? this.ShippingAddress.ToPaypalShippingAddress() : null;

            return result;
        }
    }

    public enum PayerPaymentMethod
    {
        credit_card,
        paypal
    }
}
