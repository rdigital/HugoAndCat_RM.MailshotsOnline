using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.PCL.Models.Reporting;

namespace RM.MailshotsOnline.Entities.DataModels.Reports
{
    public class TransactionsReportEntity : ITransactionsReportEntity
    {
        public string Type { get; set; }

        public string PaymentId { get; set; }

        public string SaleIdProductSku { get; set; }

        public string PaymentTimeProductName { get; set; }

        public string UserIdQuantity { get; set; }

        public string EmailAddressUnitPrice { get; set; }

        public decimal OrderSubtotalLineSubtotal { get; set; }

        public decimal OrderTaxLineTax { get; set; }

        public string OrderShippingLineTotal { get; set; }

        public string OrderTotal { get; set; }

        public string OrderCurrency { get; set; }

        public string InvoiceAddressTitle { get; set; }

        public string InvoiceAddressFirstName { get; set; }

        public string InvoiceAddressLastName { get; set; }

        public string InvoiceAddressOrganisation { get; set; }

        public string InvoiceAddressFlatNumber { get; set; }

        public string InvoiceAddressBuildingNumber { get; set; }

        public string InvoiceAddressBuildingName { get; set; }

        public string InvoiceAddressAddress1 { get; set; }

        public string InvoiceAddressAddress2 { get; set; }

        public string InvoiceAddressCity { get; set; }

        public string InvoiceAddressCounty { get; set; }

        public string InvoiceAddressPostcode { get; set; }

        public string InvoiceAddressCountry { get; set; }
    }
}
