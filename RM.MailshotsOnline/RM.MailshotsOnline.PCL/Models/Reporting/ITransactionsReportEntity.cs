namespace RM.MailshotsOnline.PCL.Models.Reporting
{
    public interface ITransactionsReportEntity
    {
        string Type { get; set; }

        string PaymentId { get; set; }

        string SaleIdProductSku { get; set; }

        string PaymentTimeProductName { get; set; }

        string UserIdQuantity { get; set; }

        string EmailAddressUnitPrice { get; set; }

        decimal OrderSubtotalLineSubtotal { get; set; }

        decimal OrderTaxLineTax { get; set; }

        string OrderShippingLineTotal { get; set; }

        string OrderTotal { get; set; }

        string OrderCurrency { get; set; }

        string InvoiceAddressTitle { get; set; }

        string InvoiceAddressFirstName { get; set; }

        string InvoiceAddressLastName { get; set; }

        string InvoiceAddressOrganisation { get; set; }

        string InvoiceAddressFlatNumber { get; set; }

        string InvoiceAddressBuildingNumber { get; set; }

        string InvoiceAddressBuildingName { get; set; }

        string InvoiceAddressAddress1 { get; set; }

        string InvoiceAddressAddress2 { get; set; }

        string InvoiceAddressCity { get; set; }

        string InvoiceAddressCounty { get; set; }

        string InvoiceAddressPostcode { get; set; }

        string InvoiceAddressCountry { get; set; }
    }
}
