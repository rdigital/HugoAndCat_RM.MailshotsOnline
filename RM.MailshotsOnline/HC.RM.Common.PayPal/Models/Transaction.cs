using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplTransaction = PayPal.Api.Transaction;
using PplAmount = PayPal.Api.Amount;
using PplDetails = PayPal.Api.Details;
using PplRelatedResources = PayPal.Api.RelatedResources;

namespace HC.RM.Common.PayPal.Models
{
    public class Transaction
    {
        /// <summary>
        /// Creates a new Transaction object
        /// </summary>
        /// <param name="currency">Three-letter currency code</param>
        /// <param name="total">Total amount for the transaction</param>
        public Transaction(string currency, decimal total)
        {
            this.Currency = currency;
            this.Total = total;
        }

        /// <summary>
        /// Creates a new Transaction object
        /// </summary>
        /// <param name="currency">Three-letter currency code</param>
        /// <param name="total">Total amount for the transaction</param>
        /// <param name="subTotal">The sub-total before tax</param>
        /// <param name="tax">The total tax</param>
        /// <param name="invoiceNumber">The invoice number for the transaction</param>
        public Transaction(string currency, decimal total, decimal subTotal, decimal tax, string invoiceNumber)
        {
            this.Currency = currency;
            this.Total = total;
            this.SubTotal = subTotal;
            this.Tax = tax;
            this.InvoiceNumber = invoiceNumber;
        }

        /// <summary>
        /// Creates a new Transaction object from a PayPal Transaction
        /// </summary>
        /// <param name="transaction">The PayPal Transaction</param>
        internal Transaction(PplTransaction transaction)
        {
            if (transaction.amount != null)
            {
                this.Currency = transaction.amount.currency;
                this.Total = decimal.Parse(transaction.amount.total);
                if (transaction.amount.details != null)
                {
                    this.SubTotal = decimal.Parse(transaction.amount.details.subtotal);
                    this.Tax = decimal.Parse(transaction.amount.details.tax);
                }
            }

            this.Description = transaction.description;
            this.InvoiceNumber = transaction.invoice_number;
            this.CustomField = transaction.custom;
            this.RelatedResources = transaction.related_resources != null 
                ? transaction.related_resources.ToRelatedResources() 
                : null;
            this.Items = transaction.item_list != null 
                ? transaction.item_list.ToPurchaseItems() 
                : null;
        }

        /// <summary>
        /// Gets or sets the Three-letter currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the Total amount for the transaction
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the Sub-total before tax
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the total Tax
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the invoice number
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets a custom field that can be set by the system and returned by PayPal
        /// </summary>
        public string CustomField { get; set; }

        /// <summary>
        /// Gets the Related resources (Authorizations, Orders, etc)
        /// </summary>
        public IEnumerable<RelatedResource> RelatedResources { get; private set; }

        /// <summary>
        /// Gets or sets the Items that the transaction is for
        /// </summary>
        public IEnumerable<PurchaseItem> Items { get; set; }

        /// <summary>
        /// Converts the Transaction to a PayPal Transaction object
        /// </summary>
        /// <returns>PayPal Transaction object</returns>
        internal PplTransaction ToPaypalTransaction()
        {
            PplTransaction result = new PplTransaction();

            result.amount = new PplAmount();
            result.amount.currency = this.Currency;
            result.amount.total = this.Total.ToString("F");
            result.amount.details = new PplDetails();
            result.amount.details.tax = this.Tax.ToString("F");
            result.amount.details.subtotal = this.SubTotal.ToString("F");
            result.custom = this.CustomField;
            result.description = this.Description;
            result.related_resources = this.RelatedResources != null 
                ? this.RelatedResources.ToPplRelatedResources() 
                : null;
            result.item_list = this.Items != null 
                ? this.Items.ToPplItemList() 
                : null;

            return result;
        }
    }
}
