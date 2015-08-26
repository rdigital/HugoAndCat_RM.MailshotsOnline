using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplPayment = PayPal.Api.Payment;
using RedirectUrls = PayPal.Api.RedirectUrls;

namespace HC.RM.Common.PayPal.Models
{
    public class Payment
    {
        public Payment()
        {

        }

        public Payment(
            PaymentIntent intent, 
            string returnUrl, 
            string cancelUrl, 
            decimal totalAmount, 
            decimal tax, 
            decimal subTotal, 
            string currency, 
            string invoiceNumber,
            Payer payer)
            : this(intent, returnUrl, cancelUrl, totalAmount, tax, subTotal, currency, invoiceNumber)
        {
            this.Payer = payer;
        }

        public Payment(PaymentIntent intent, string returnUrl, string cancelUrl, decimal totalAmount, decimal tax, decimal subTotal, string currency, string invoiceNumber)
        {
            this.Intent = intent;
            this.ReturnUrl = returnUrl;
            this.CancelUrl = cancelUrl;
            var transactions = new List<Transaction>();
            transactions.Add(new Transaction(currency, totalAmount, subTotal, tax, invoiceNumber));
            this.Transactions = transactions;
        }

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
            var transaction = pplPayment.transactions.FirstOrDefault();
            transaction.related_resources.First().order.
        }

        public string Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public PaymentState State { get; set; }

        public PaymentIntent Intent { get; set; }

        public string ReturnUrl { get; set; }

        public string CancelUrl { get; set; }

        public Payer Payer { get; set; }

        public IEnumerable<HateoasLink> Links { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }

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

            return pplPayment;
        }
    }

    public enum PaymentIntent
    {
        Sale = 1,
        Authorisation = 2,
        Order = 3
    }

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
