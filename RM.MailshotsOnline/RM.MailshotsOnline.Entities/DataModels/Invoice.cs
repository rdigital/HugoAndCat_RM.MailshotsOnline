using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace RM.MailshotsOnline.Entities.DataModels
{
    public class Invoice : IInvoice
    {
        private Campaign _campaign;

        private decimal _dataRentalCost;

        private decimal _postageCost;

        private decimal _printingCost;

        private decimal _subTotal;

        private decimal _total;

        private decimal _totalTax;

        private Address _billingAddress;

        private ICollection<InvoiceLineItem> _lineItems;

        /// <summary>
        /// Gets or sets the Invoice ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InvoiceId { get; set; }


        /// <summary>
        /// Gets or sets the associated Campaign
        /// </summary>
        [ForeignKey("CampaignId")]
        public Campaign Campaign
        {
            get { return _campaign; }
            set { _campaign = value; }
        }

        /// <summary>
        /// Gets or sets the Campaign ID
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// The date the invoice was created
        /// </summary>
        public DateTime CreatedDate { get { return CreatedUtc; } }

        /// <summary>
        /// The DB generated created date
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonIgnore]
        public DateTime CreatedUtc { get; private set; }

        /// <summary>
        /// The date the invoice was updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the invoice
        /// </summary>
        public Enums.InvoiceStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the PayPal Payment ID
        /// </summary>
        [MaxLength(64)]
        public string PaypalOrderId { get; set; }

        /// <summary>
        /// Gets or sets the PayPal Order ID
        /// </summary>
        [MaxLength(64)]
        public string PaypalPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the PayPal approval URL
        /// </summary>
        [MaxLength(2048)]
        [JsonIgnore]
        public string PaypalApprovalUrl { get; set; }

        /// <summary>
        /// Gets or sets the invoice order number
        /// </summary>
        [MaxLength(13)]
        [Index]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the line items
        /// </summary>
        [JsonIgnore]
        [InverseProperty("Invoice")]
        public virtual ICollection<InvoiceLineItem> LineItems
        {
            get
            {
                return _lineItems;
            }

            set
            {
                _lineItems = value;
            }
        }

        /// <summary>
        /// Gets the calculated sub total
        /// </summary>
        public decimal SubTotal
        {
            get
            {
                _subTotal = 0; // PrintingCost + PostageCost + DataRentalCost + ServiceFee;
                if (_lineItems != null)
                {
                    _subTotal = _lineItems.Sum(i => i.SubTotal);
                }
                return _subTotal;
            }

            private set
            {
                _subTotal = value;
            }
        }

        /// <summary>
        /// Gets the calculated total
        /// </summary>
        public decimal Total
        {
            get
            {
                _total = SubTotal + TotalTax;
                return _total;
            }

            private set
            {
                _total = value;
            }
        }

        /// <summary>
        /// Gets the calculated total tax
        /// </summary>
        public decimal TotalTax
        {
            get
            {
                _totalTax = 0; // TaxRate * SubTotal;
                if (_lineItems != null)
                {
                    _totalTax = _lineItems.Sum(i => i.TaxTotal);
                }
                return _totalTax;
            }

            private set
            {
                _totalTax = value;
            }
        }

        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        public Address BillingAddress
        {
            get { return _billingAddress; }
            set { _billingAddress = value; }
        }

        /// <summary>
        /// Gets or sets the invoice PDF blob status
        /// </summary>
        public string InvoicePdfBlobReference { get; set; }

        /// <summary>
        /// Gets or sets the date the invoice was paid
        /// </summary>
        public DateTime? PaidDate { get; set; }

        /// <summary>
        /// Gets or sets the date the invoice was cancelled
        /// </summary>
        public DateTime? CancelledDate { get; set; }

        /// <summary>
        /// Gets the key invoice information as XML
        /// </summary>
        /// <returns>XML string</returns>
        public string ToXmlString()
        {
            XDocument xdoc = new XDocument();
            XElement root = new XElement("invoice");
            if (BillingAddress != null)
            {
                root.Add(new XElement("billing-address", BillingAddress.ToString()));
            }
            root.Add(new XElement("order-number", OrderNumber));
            root.Add(new XElement("order-date", CreatedDate.ToString("dd-MM-yyyy")));
            if (PaidDate.HasValue)
            {
                root.Add(new XElement("paid-date", PaidDate.Value.ToString("dd-MM-yyyy")));
            }
            root.Add(new XElement("sub-total", SubTotal.ToString("F2")));
            root.Add(new XElement("tax-total", TotalTax.ToString("F2")));
            root.Add(new XElement("total", Total.ToString("F2")));
            var lineItems = new XElement("line-items");
            foreach (var lineItem in LineItems)
            {
                var element = new XElement("line-item");
                element.Add(new XElement("name", lineItem.Name));
                element.Add(new XElement("subtitle", lineItem.SubTitle));
                element.Add(new XElement("quantity", lineItem.Quantity.ToString()));
                element.Add(new XElement("unit-cost", lineItem.UnitCost.ToString("F2")));
                element.Add(new XElement("sub-total", lineItem.SubTotal.ToString("F2")));
                element.Add(new XElement("tax-rate", lineItem.TaxRate.ToString("F2")));
                element.Add(new XElement("tax-total", lineItem.TaxTotal.ToString("F2")));
                element.Add(new XElement("total", lineItem.Total.ToString("F2")));
                lineItems.Add(element);
            }
            root.Add(lineItems);

            xdoc.Add(root);
            return xdoc.ToString();
        }

        #region Explicit interface implementation

        ICampaign IInvoice.Campaign
        {
            get { return (ICampaign)_campaign; }
            set { _campaign = (Campaign)value; }
        }

        ICollection<IInvoiceLineItem> IInvoice.LineItems
        {
            get
            {
                if (_lineItems == null)
                {
                    return null;
                }

                var newSet = _lineItems.Cast<IInvoiceLineItem>();
                return newSet.ToList();
            }

            set { _lineItems = value.Cast<InvoiceLineItem>().ToList(); }
        }

        IAddress IInvoice.BillingAddress
        {
            get { return (IAddress)_billingAddress; }
            set { _billingAddress = (Address)value; }
        }

        #endregion
    }
}
