using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IInvoiceLineItem
    {
        /// <summary>
        /// Gets or sets the ID of the invoice line item
        /// </summary>
        Guid InvoiceLineItemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent invoice
        /// </summary>
        Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the parent Invoice object
        /// </summary>
        IInvoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets the name of the item
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the category for this line item
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the line-item subtitle
        /// </summary>
        string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the item
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the cost per unit
        /// </summary>
        decimal UnitCost { get; set; }

        /// <summary>
        /// Gets the sub total (before tax)
        /// </summary>
        decimal SubTotal { get; }

        /// <summary>
        /// Gets or sets the tax rate
        /// </summary>
        decimal TaxRate { get; set; }

        /// <summary>
        /// Gets the total tax
        /// </summary>
        decimal TaxTotal { get; }

        /// <summary>
        /// Gets the total cost (including tax)
        /// </summary>
        decimal Total { get; }
    }
}
