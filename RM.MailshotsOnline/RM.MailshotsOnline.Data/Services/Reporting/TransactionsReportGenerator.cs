using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RM.MailshotsOnline.Entities.DataModels;
using RM.MailshotsOnline.Entities.DataModels.Reports;
using RM.MailshotsOnline.PCL.Models;
using RM.MailshotsOnline.PCL.Models.Reporting;
using RM.MailshotsOnline.PCL.Services;
using RM.MailshotsOnline.PCL.Services.Reporting;

namespace RM.MailshotsOnline.Data.Services.Reporting
{
    public class TransactionsReportGenerator : ITransactionsReportGenerator
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMembershipService _membershipService;

        public TransactionsReportGenerator(IInvoiceService invoiceService, IMembershipService membershipService)
        {
            _invoiceService = invoiceService;
            _membershipService = membershipService;
        }

        public ITransactionsReport Generate()
        {
            var invoices = _invoiceService.GetPaidInvoices(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1));

            var items = new List<ITransactionsReportEntity>();

            foreach (var invoice in invoices)
            {
                var member = _membershipService.GetMemberById(invoice.Campaign.UserId);
                var transactionID = !string.IsNullOrEmpty(invoice.PaypalCaptureTransactionId)
                    ? invoice.PaypalCaptureTransactionId
                    : invoice.PaypalPaymentId;

                items.Add(new TransactionsReportEntity()
                {
                    Type = "Order",
                    PaymentId = transactionID,
                    SaleIdProductSku = invoice.OrderNumber,
                    PaymentTimeProductName = invoice.CreatedDate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                    UserIdQuantity = invoice.Campaign.UserId.ToString(),
                    EmailAddressUnitPrice = invoice.BillingEmail,
                    OrderSubtotalLineSubtotal = invoice.SubTotal,
                    OrderTaxLineTax = invoice.TotalTax,
                    OrderTotal = invoice.Total.ToString(CultureInfo.InvariantCulture),
                    OrderCurrency = "",
                    InvoiceAddressTitle = invoice.BillingAddress.Title,
                    InvoiceAddressFirstName = invoice.BillingAddress.FirstName,
                    InvoiceAddressLastName = invoice.BillingAddress.LastName,
                    InvoiceAddressOrganisation = member.OrganisationName,
                    InvoiceAddressFlatNumber = invoice.BillingAddress.FlatNumber,
                    InvoiceAddressBuildingNumber = invoice.BillingAddress.BuildingNumber,
                    InvoiceAddressBuildingName = invoice.BillingAddress.BuildingName,
                    InvoiceAddressAddress1 = invoice.BillingAddress.Address1,
                    InvoiceAddressAddress2 = invoice.BillingAddress.Address2,
                    InvoiceAddressCity = invoice.BillingAddress.City,
                    InvoiceAddressPostcode = invoice.BillingAddress.Postcode,
                    InvoiceAddressCountry = invoice.BillingAddress.Country
                });

                foreach (var lineItem in invoice.LineItems)
                {
                    items.Add(new TransactionsReportEntity()
                    {
                        Type = "LineItems",
                        PaymentId = transactionID,
                        SaleIdProductSku = lineItem.ProductSku,
                        PaymentTimeProductName = string.IsNullOrEmpty(lineItem.SubTitle) ? lineItem.Product.Name : $"{lineItem.Product.Name} ({lineItem.SubTitle})",
                        UserIdQuantity = lineItem.Quantity.ToString(),
                        EmailAddressUnitPrice = lineItem.UnitCost.ToString(CultureInfo.InvariantCulture),
                        OrderSubtotalLineSubtotal = lineItem.SubTotal,
                        OrderTaxLineTax = lineItem.TaxTotal,
                        OrderShippingLineTotal = lineItem.Total.ToString(CultureInfo.InvariantCulture),
                    });
                }
            }

            var report = new TransactionsReport() {CreatedDate = DateTime.UtcNow, Transactions = items};

            return report;
        }
    }
}
