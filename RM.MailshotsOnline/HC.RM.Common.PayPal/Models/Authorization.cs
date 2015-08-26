using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplAuthorization = PayPal.Api.Authorization;
using PplAmount = PayPal.Api.Amount;

namespace HC.RM.Common.PayPal.Models
{
    public class Authorization : RelatedResource
    {
        /// <summary>
        /// Creates a new Authorization object from a PayPal Authorization
        /// </summary>
        /// <param name="authorization">PayPal Authorization object</param>
        internal Authorization(PplAuthorization authorization)
        {
            if (authorization.amount != null)
            {
                this.AmountCurrency = authorization.amount.currency;
                this.AmountTotal = decimal.Parse(authorization.amount.total);
            }

            this.CreateTime = DateTime.Parse(authorization.create_time);
            this.Id = authorization.id;
            this.Links = authorization.links.ToHateoasLinks();
            this.ParentPaymentId = authorization.parent_payment;
            this.PaymentMode = (AuthorizationPaymentMode)Enum.Parse(typeof(AuthorizationPaymentMode), authorization.payment_mode);
            this.State = (AuthorizationState)Enum.Parse(typeof(AuthorizationState), authorization.state);
            this.UpdateTime = DateTime.Parse(authorization.update_time);
            this.ValidUntil = DateTime.Parse(authorization.valid_until);
        }

        /// <summary>
        /// The time the Authorization is valid until
        /// </summary>
        public DateTime ValidUntil { get; private set; }

        /// <summary>
        /// The selected Payment Mode
        /// </summary>
        public AuthorizationPaymentMode PaymentMode { get; private set; }

        /// <summary>
        /// The Authorization state
        /// </summary>
        public AuthorizationState State { get; private set; }

        /// <summary>
        /// Converts the Authorization to a PayPal Authorization
        /// </summary>
        /// <returns>PayPal Authorization</returns>
        internal PplAuthorization ToPaypalAuthorization()
        {
            var result = new PplAuthorization();
            result.amount = new PplAmount() { currency = this.AmountCurrency, total = AmountTotal.ToString("F") };
            result.create_time = this.CreateTime.ToUniversalTime().ToString("O");
            result.id = this.Id;
            result.links = this.Links.ToPplLinks();
            result.parent_payment = this.ParentPaymentId;
            result.payment_mode = this.PaymentMode.ToString();
            result.state = this.State.ToString();
            result.update_time = this.UpdateTime.ToUniversalTime().ToString("O");
            result.valid_until = this.ValidUntil.ToUniversalTime().ToString("O");

            return result;
        }
    }

    /// <summary>
    /// Authorization State
    /// </summary>
    public enum AuthorizationState
    {
        pending,
        authroized,
        captured,
        partiall_captured,
        expired,
        voided
    }

    /// <summary>
    /// Authorization payment mode
    /// </summary>
    public enum AuthorizationPaymentMode
    {
        INSTANT_TRANSFER,
        MANUAL_BANK_TRANSFER,
        DELAYED_TRANSFER,
        ECHECK
    }
}
