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
        public string PaypalOrderId { get; set; }

        /// <summary>
        /// Gets or sets the PayPal Order ID
        /// </summary>
        public string PaypalPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the PayPal approval URL
        /// </summary>
        public string PaypalApprovalUrl { get; set; }

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
        /// Gets the calculated Data Rental cost
        /// </summary>
        /*public decimal DataRentalCost
        {
            get
            {

                if (DataRentalCount > 0)
                {
                    _dataRentalCost = DataRentalFlatFee + (DataRentalCount * DataRentalRate);
                }
                else
                {
                    _dataRentalCost = 0;
                }

                return _dataRentalCost;
            }

            private set
            {
                _dataRentalCost = value;
            }
        }

        /// <summary>
        /// Gets the data rental count
        /// </summary>
        public int DataRentalCount { get; set; }

        /// <summary>
        /// Gets or sets the Data Rental flat fee
        /// </summary>
        public decimal DataRentalFlatFee { get; set; }

        /// <summary>
        /// Gets or sets the data renal rate
        /// </summary>
        public decimal DataRentalRate { get; set; }

        /// <summary>
        /// Gets the calculated postage cost
        /// </summary>
        public decimal PostageCost
        {
            get
            {
                _postageCost = PostageRate * PrintCount;
                return _postageCost;
            }

            private set
            {
                _postageCost = value;
            }
        }

        public decimal PostageRate { get; set; }

        /// <summary>
        /// Gets or sets the Print Count
        /// </summary>
        public int PrintCount { get; set; }

        /// <summary>
        /// Gets the calculated postage cost
        /// </summary>
        public decimal PrintingCost
        {
            get
            {
                _printingCost = PrintCount * PrintingRate;
                return _printingCost;
            }

            private set
            {
                _printingCost = value;
            }
        }

        /// <summary>
        /// Gets or sets the postage rate
        /// </summary>
        public decimal PrintingRate { get; set; }

        /// <summary>
        /// Gets or sets the Service Fee
        /// </summary>
        public decimal ServiceFee { get; set; } */

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
        /// Gets or sets the tax rate applied
        /// </summary>
        /*public decimal TaxRate { get; set; }*/

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

        public Address BillingAddress
        {
            get { return _billingAddress; }
            set { _billingAddress = value; }
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
