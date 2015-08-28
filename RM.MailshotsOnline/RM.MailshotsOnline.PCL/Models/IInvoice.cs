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
        /// Gets or sets the Print Count
        /// </summary>
        int PrintCount { get; set; }

        /// <summary>
        /// Gets or sets the Data Rental flat fee
        /// </summary>
        decimal DataRentalFlatFee { get; set; }

        /// <summary>
        /// Gets the calculated Data Rental cost
        /// </summary>
        decimal DataRentalCost { get; }

        /// <summary>
        /// Gets the data rental count
        /// </summary>
        int DataRentalCount { get; set; }

        /// <summary>
        /// Gets or sets the data renal rate
        /// </summary>
        decimal DataRentalRate { get; set; }

        /// <summary>
        /// Gets or sets the tax rate applied
        /// </summary>
        decimal TaxRate { get; set; }

        /// <summary>
        /// Gets the calculated total tax
        /// </summary>
        decimal TotalTax { get; }

        /// <summary>
        /// Gets the calculated postage cost
        /// </summary>
        decimal PostageCost { get; }

        /// <summary>
        /// Gets or sets the postage rate
        /// </summary>
        decimal PostageRate { get; set; }

        /// <summary>
        /// Gets or sets the Service Fee
        /// </summary>
        decimal ServiceFee { get; set; }

        /// <summary>
        /// Gets or sets the printing rate
        /// </summary>
        decimal PrintingRate { get; set; }

        /// <summary>
        /// Gets the calculated printing cost
        /// </summary>
        decimal PrintingCost { get; }

        /// <summary>
        /// Gets the calculated sub total
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
    }
}
