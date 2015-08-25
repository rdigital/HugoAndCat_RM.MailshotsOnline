using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplTransaction = PayPal.Api.Transaction;
using PplAmount = PayPal.Api.Amount;
using PplDetails = PayPal.Api.Details;

namespace HC.RM.Common.PayPal.Models
{
    public class Transaction
    {
        public Transaction()
        {

        }

        public Transaction(string currency, decimal total, decimal subTotal, decimal tax, string invoiceNumber)
        {
            this.Currency = currency;
            this.Total = total;
            this.SubTotal = subTotal;
            this.Tax = tax;
            this.InvoiceNumber = invoiceNumber;
        }

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
        }

        public string Currency { get; set; }

        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public string Description { get; set; }

        public string InvoiceNumber { get; set; }

        public string CustomField { get; set; }

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

            return result;
        }
    }
}
