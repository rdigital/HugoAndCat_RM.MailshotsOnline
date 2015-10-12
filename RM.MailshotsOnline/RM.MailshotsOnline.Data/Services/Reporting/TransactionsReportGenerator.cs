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

                items.Add(new TransactionsReportEntity()
                {
                    Type = "Order",
                    PaymentId = invoice.PaypalPaymentId,
                    SaleIdProductSku = invoice.OrderNumber,
                    PaymentTimeProductName = invoice.CreatedDate.ToString("yyyyMMdd"),
                    UserIdQuantity = invoice.Campaign.UserId.ToString(),
                    EmailAddressUnitPrice = invoice.BillingEmail,
                    OrderSubtotalLineSubtotal = invoice.SubTotal,
                    OrderTaxLineTax = invoice.TotalTax,
                    OrderTotal = invoice.Total.ToString(CultureInfo.InvariantCulture),
                    OrderCurrency = "",
                    InvoiceAddressTitle = member.Title,
                    InvoiceAddressFirstName = member.FirstName,
                    InvoiceAddressLastName = member.LastName,
                    InvoiceAddressOrganisation = member.OrganisationName,
                    InvoiceAddressFlatNumber = member.FlatNumber,
                    InvoiceAddressBuildingNumber = member.BuildingNumber,
                    InvoiceAddressBuildingName = member.BuildingName,
                    InvoiceAddressAddress1 = member.Address1,
                    InvoiceAddressAddress2 = member.Address2,
                    InvoiceAddressCity = member.City,
                    InvoiceAddressPostcode = member.Postcode,
                    InvoiceAddressCountry = member.Country
                });

                foreach (var lineItem in invoice.LineItems)
                {
                    items.Add(new TransactionsReportEntity()
                    {
                        Type = "LineItems",
                        PaymentId = invoice.PaypalPaymentId,
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
