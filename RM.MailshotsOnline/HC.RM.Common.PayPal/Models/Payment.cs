using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplPayment = PayPal.Api.Payment;
using RedirectUrls = PayPal.Api.RedirectUrls;

namespace HC.RM.Common.PayPal.Models
{
    /// <summary>
    /// Represents a Payment object
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Creates a new Payment object
        /// </summary>
        /// <param name="intent">The intent for the Payment</param>
        /// <param name="payer">The Payer object</param>
        /// <param name="transaction">The first Transaction object</param>
        /// <param name="returnUrl">The Return URL</param>
        /// <param name="cancelUrl">The Cancel URL</param>
        public Payment(PaymentIntent intent, Payer payer, Transaction transaction, string returnUrl, string cancelUrl)
        {
            this.Intent = intent;
            this.Payer = payer;
            var transactionsList = new List<Transaction>();
            transactionsList.Add(transaction);
            this.Transactions = transactionsList;
        }

        /// <summary>
        /// Creates a new Payment object
        /// </summary>
        /// <param name="cartId">The site Cart ID</param>
        /// <param name="intent">The intent for the Payment</param>
        /// <param name="returnUrl">The Return URL</param>
        /// <param name="cancelUrl">The Cancel URL</param>
        /// <param name="totalAmount">The total amount to charge</param>
        /// <param name="tax">The tax being charged</param>
        /// <param name="subTotal">The sub-total before tax being charged</param>
        /// <param name="currency">Three-letter currency code</param>
        /// <param name="invoiceNumber">The Invoice number</param>
        /// <param name="payer">The Payer object</param>
        public Payment(
            string cartId,
            PaymentIntent intent, 
            string returnUrl, 
            string cancelUrl, 
            decimal totalAmount, 
            decimal tax, 
            decimal subTotal, 
            string currency, 
            string invoiceNumber,
            Payer payer)
            : this(cartId, intent, returnUrl, cancelUrl, totalAmount, tax, subTotal, currency, invoiceNumber)
        {
            this.Payer = payer;
        }

        /// <summary>
        /// Creates a new Payment object
        /// </summary>
        /// <param name="cartId">The site Cart ID</param>
        /// <param name="intent">The intent for the Payment</param>
        /// <param name="returnUrl">The Return URL</param>
        /// <param name="cancelUrl">The Cancel URL</param>
        /// <param name="totalAmount">The total amount to charge</param>
        /// <param name="tax">The tax being charged</param>
        /// <param name="subTotal">The sub-total before tax being charged</param>
        /// <param name="currency">Three-letter currency code</param>
        /// <param name="invoiceNumber">The Invoice number</param>
        public Payment(string cartId, PaymentIntent intent, string returnUrl, string cancelUrl, decimal totalAmount, decimal tax, decimal subTotal, string currency, string invoiceNumber)
        {
            this.CartId = cartId;
            this.Intent = intent;
            this.ReturnUrl = returnUrl;
            this.CancelUrl = cancelUrl;
            var transactions = new List<Transaction>();
            transactions.Add(new Transaction(currency, totalAmount, subTotal, tax, invoiceNumber));
            this.Transactions = transactions;
        }

        /// <summary>
        /// Creates a Payment object from a PayPal Payment object
        /// </summary>
        /// <param name="pplPayment">The PayPal Payment object</param>
        internal Payment(PplPayment pplPayment)
        {
            this.Id = pplPayment.id;
            this.CreateTime = DateTime.Parse(pplPayment.create_time);
            this.UpdateTime = DateTime.Parse(pplPayment.update_time);
            this.Intent = (PaymentIntent)Enum.Parse(typeof(PaymentIntent), pplPayment.intent);
            this.State = (PaymentState)Enum.Parse(typeof(PaymentState), pplPayment.state);
            this.ReturnUrl = pplPayment.redirect_urls != null ? pplPayment.redirect_urls.return_url : null;
            this.CancelUrl = pplPayment.redirect_urls != null ? pplPayment.redirect_urls.cancel_url : null;
            this.Payer = pplPayment.payer != null ? new Payer(pplPayment.payer) : null;
            this.Links = pplPayment.links != null ? pplPayment.links.ToHateoasLinks() : null;
            this.Transactions = pplPayment.transactions != null ? pplPayment.transactions.ToTransactions() : null;
            this.CartId = pplPayment.cart;
        }

        /// <summary>
        /// Gets the Payment ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the Create time
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// Gets the Update time
        /// </summary>
        public DateTime UpdateTime { get; private set; }

        /// <summary>
        /// Gets the Payment State
        /// </summary>
        public PaymentState State { get; private set; }

        /// <summary>
        /// Gets or sets the intent for the Payment (required)
        /// </summary>
        public PaymentIntent Intent { get; set; }

        /// <summary>
        /// Gets or sets the Return URL (required if Payer's method is Paypal)
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the Cancel URL (required if Payer's method is Paypal)
        /// </summary>
        public string CancelUrl { get; set; }

        /// <summary>
        /// Gets or sets the Payer (required)
        /// </summary>
        public Payer Payer { get; set; }

        /// <summary>
        /// Gets or sets the site's Cart ID
        /// </summary>
        public string CartId { get; set; }

        /// <summary>
        /// Gets the HATEOS Links
        /// </summary>
        public IEnumerable<HateoasLink> Links { get; private set; }

        /// <summary>
        /// Gets or sets the transactions of the Payment (required)
        /// </summary>
        public IEnumerable<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets the Approval URL for the payment
        /// </summary>
        public string ApprovalUrl
        {
            get
            {
                if (Links != null)
                {
                    var approvalUrl = this.Links.FirstOrDefault(l => l.Rel == "approval_url" && l.Method == "REDIRECT");
                    if (approvalUrl != null)
                    {
                        return approvalUrl.Href;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the Token used in the ApprovalUrl link
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Converst the Payment object to a PayPal Payment
        /// </summary>
        /// <returns>PayPal Payment</returns>
        internal PplPayment ToPaypalPayment()
        {
            PplPayment pplPayment = new PplPayment();

            pplPayment.create_time = this.CreateTime.ToUniversalTime().ToString("O");
            pplPayment.id = this.Id;
            pplPayment.intent = this.Intent.ToString();
            pplPayment.links = this.Links.ToPplLinks();
            pplPayment.payer = this.Payer.ToPaypalPayer();
            pplPayment.redirect_urls = new RedirectUrls() { cancel_url = this.CancelUrl, return_url = this.ReturnUrl };
            pplPayment.state = this.State.ToString();
            pplPayment.token = this.Token;
            pplPayment.update_time = this.UpdateTime.ToUniversalTime().ToString("O");
            pplPayment.transactions = this.Transactions.ToPplTransactions();
            pplPayment.cart = this.CartId;

            return pplPayment;
        }
    }

    /// <summary>
    /// Payment intent
    /// </summary>
    public enum PaymentIntent
    {
        Sale = 1,
        Authorisation = 2,
        Order = 3
    }

    /// <summary>
    /// Payment State
    /// </summary>
    public enum PaymentState
    {
        created,
        approved,
        failed,
        canceled,
        expired,
        pending
    }
}
