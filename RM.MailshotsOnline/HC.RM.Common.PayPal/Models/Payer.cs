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
        /// <summary>
        /// Creates a new Payer object
        /// </summary>
        /// <param name="paymentMethod">The Payment method</param>
        public Payer(PayerPaymentMethod paymentMethod)
        {
            this.PaymentMethod = paymentMethod;
        }

        /// <summary>
        /// Creates a new Payer object from a PayPal payer object
        /// </summary>
        /// <param name="payer"></param>
        internal Payer(PplPayer payer)
        {
            this.PaymentMethod = (PayerPaymentMethod)Enum.Parse(typeof(PayerPaymentMethod), payer.payment_method);

            if (payer.payer_info != null)
            {
                this.Id = payer.payer_info.payer_id;
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
        }

        /// <summary>
        /// The Payer ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The Payment method
        /// </summary>
        public PayerPaymentMethod PaymentMethod { get; private set; }

        /// <summary>
        /// The Payer's email
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// The Payer's Salutation
        /// </summary>
        public string Salutation { get; private set; }

        /// <summary>
        /// The Payer's first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// The Payer's middle name
        /// </summary>
        public string MiddleName { get; private set; }

        /// <summary>
        /// The Payer's last name
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// The Payer's suffix
        /// </summary>
        public string Suffix { get; private set; }

        /// <summary>
        /// The Payer's phone number
        /// </summary>
        public string Phone { get; private set; }

        /// <summary>
        /// Country code for the Payer's phone number
        /// </summary>
        public string CountryCode { get; private set; }

        /// <summary>
        /// The Payer's Shipping Address
        /// </summary>
        public Address ShippingAddress { get; private set; }

        /// <summary>
        /// The Payer's billing address
        /// </summary>
        public Address BillingAddress { get; private set; }

        /// <summary>
        /// Converts the Payer object to a PayPal Payer object
        /// </summary>
        /// <returns>PayPal Payer object</returns>
        internal PplPayer ToPaypalPayer()
        {
            var result = new PplPayer();

            result.payment_method = this.PaymentMethod.ToString();

            if (this.PaymentMethod == PayerPaymentMethod.credit_card)
            {
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
            }

            return result;
        }
    }

    /// <summary>
    /// The Payment Methods
    /// </summary>
    public enum PayerPaymentMethod
    {
        credit_card,
        paypal
    }
}
