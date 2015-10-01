using RM.MailshotsOnline.PCL.Models;
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

        private Product _product;

        private string _name;

        private string _category;

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
            get { return _invoice; }
            set { _invoice = value; }
        }

        /// <summary>
        /// The Product SKU
        /// </summary>
        [MaxLength(128)]
        public string ProductSku { get; set; }

        /// <summary>
        /// The Product purchased
        /// </summary>
        [ForeignKey("ProductSku")]
        public virtual Product Product
        {
            get { return _product; }
            set { _product = value; }
        }

        /// <summary>
        /// Gets the product name
        /// </summary>
        [MaxLength(1024)]
        public string Name
        {
            get
            {
                _name = _product != null ? _product.Name : null;
                return _name;
            }

            private set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets the product category
        /// </summary>
        [MaxLength(1024)]
        public string Category
        {
            get
            {
                _category = _product != null ? _product.Category : null;
                return _category;
            }

            private set
            {
                _category = value;
            }
        }

        /// <summary>
        /// Gets or sets the line-item subtitle
        /// </summary>
        [MaxLength(1024)]
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

        IProduct IInvoiceLineItem.Product
        {
            get { return (IProduct)_product; }
            set { _product = (Product)value; }
        }

        #endregion
    }
}
