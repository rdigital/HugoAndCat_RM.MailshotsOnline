using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Models
{
    public interface IInvoice
    {
        /// <summary>
        /// Gets or sets the Invoice ID
        /// </summary>
        Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the Campaign ID
        /// </summary>
        Guid CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the associated Campaign
        /// </summary>
        ICampaign Campaign { get; set; }

        /// <summary>
        /// Gets the calculated total tax
        /// </summary>
        decimal TotalTax { get; }

        /// <summary>
        /// Gets the calculated sub total (before tax)
        /// </summary>
        decimal SubTotal { get; }

        /// <summary>
        /// Gets the calculated total
        /// </summary>
        decimal Total { get; }

        /// <summary>
        /// Gets or sets the status of the invoice
        /// </summary>
        Enums.InvoiceStatus Status { get; set; }

        /// <summary>
        /// The date the invoice was created
        /// </summary>
        DateTime CreatedDate { get; }

        /// <summary>
        /// The date the campaign was updated
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the PayPal Payment ID
        /// </summary>
        string PaypalPaymentId { get; set; }

        /// <summary>
        /// Gets or sets the PayPal Order ID
        /// </summary>
        string PaypalOrderId { get; set; }

        /// <summary>
        /// Gets or sets the PayPal approval URL
        /// </summary>
        string PaypalApprovalUrl { get; set; }

        /// <summary>
        /// The line items for the invoice
        /// </summary>
        ICollection<IInvoiceLineItem> LineItems { get; set; }
    }
}
