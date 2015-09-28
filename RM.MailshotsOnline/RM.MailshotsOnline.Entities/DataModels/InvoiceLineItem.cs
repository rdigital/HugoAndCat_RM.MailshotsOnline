﻿using RM.MailshotsOnline.PCL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.MailshotsOnline.Entities.DataModels
{
    public class InvoiceLineItem : IInvoiceLineItem
    {
        private Invoice _invoice;

        private decimal _subTotal;

        private decimal _taxTotal;

        private decimal _total;

        /// <summary>
        /// Gets or sets the ID of the invoice line item
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InvoiceLineItemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent invoice
        /// </summary>
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the parent Invoice object
        /// </summary>
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice
        {
            get
            {
                return _invoice;
            }

            set
            {
                _invoice = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the category for this line item
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the line-item subtitle
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the cost per unit
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Gets the sub total (before tax)
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal SubTotal
        {
            get
            {
                _subTotal = this.Quantity * this.UnitCost;
                return _subTotal;
            }

            private set
            {
                _subTotal = value;
            }
        }

        /// <summary>
        /// Gets or sets the tax rate
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Gets the total tax
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal TaxTotal
        {
            get
            {
                _taxTotal = this.TaxRate * this.SubTotal;
                return _taxTotal;
            }

            private set
            {
                _taxTotal = value;
            }
        }

        /// <summary>
        /// Gets the total cost (including tax)
        /// </summary>
        [Column(TypeName = "Money")]
        public decimal Total
        {
            get
            {
                _total = this.SubTotal + this.TaxTotal;
                return _total;
            }

            private set
            {
                _total = value;
            }
        }

        #region Explicit interface implementation

        /// <summary>
        /// Gets or sets the parent invoice object
        /// </summary>
        IInvoice IInvoiceLineItem.Invoice
        {
            get { return (IInvoice)_invoice; }
            set { _invoice = (Invoice)value; }
        }

        #endregion
    }
}
