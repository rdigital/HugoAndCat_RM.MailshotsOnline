namespace RM.MailshotsOnline.Data.Migrations
{
    using Extensions;
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddOrderNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "OrderNumber", c => c.String(maxLength: 13));

            this.DeleteDefaultContstraint("dbo.Campaigns", "Name");
            AlterColumn("dbo.Campaigns", "Name", c => c.String(nullable: false, maxLength: 512));

            this.DeleteDefaultContstraint("dbo.Campaigns", "Notes");
            AlterColumn("dbo.Campaigns", "Notes", c => c.String(maxLength: 1024));

            this.DeleteDefaultContstraint("dbo.Campaigns", "SystemNotes");
            AlterColumn("dbo.Campaigns", "SystemNotes", c => c.String(maxLength: 2048));

            this.DeleteDefaultContstraint("dbo.DataSearches", "Name");
            AlterColumn("dbo.DataSearches", "Name", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.DataSearches", "SearchCriteria");
            AlterColumn("dbo.DataSearches", "SearchCriteria", c => c.String(maxLength: 2048));

            this.DeleteDefaultContstraint("dbo.DataSearches", "ThirdPartyIdentifier");
            AlterColumn("dbo.DataSearches", "ThirdPartyIdentifier", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "PaypalOrderId");
            AlterColumn("dbo.Invoices", "PaypalOrderId", c => c.String(maxLength: 64));

            this.DeleteDefaultContstraint("dbo.Invoices", "PaypalPaymentId");
            AlterColumn("dbo.Invoices", "PaypalPaymentId", c => c.String(maxLength: 64));

            this.DeleteDefaultContstraint("dbo.Invoices", "PaypalApprovalUrl");
            AlterColumn("dbo.Invoices", "PaypalApprovalUrl", c => c.String(maxLength: 2048));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_FirstName");
            AlterColumn("dbo.Invoices", "BillingAddress_FirstName", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_LastName");
            AlterColumn("dbo.Invoices", "BillingAddress_LastName", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_FlatNumber");
            AlterColumn("dbo.Invoices", "BillingAddress_FlatNumber", c => c.String(maxLength: 64));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_BuildingNumber");
            AlterColumn("dbo.Invoices", "BillingAddress_BuildingNumber", c => c.String(maxLength: 64));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_BuildingName");
            AlterColumn("dbo.Invoices", "BillingAddress_BuildingName", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_Address1");
            AlterColumn("dbo.Invoices", "BillingAddress_Address1", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_Address2");
            AlterColumn("dbo.Invoices", "BillingAddress_Address2", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_City");
            AlterColumn("dbo.Invoices", "BillingAddress_City", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_Country");
            AlterColumn("dbo.Invoices", "BillingAddress_Country", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Invoices", "BillingAddress_Postcode");
            AlterColumn("dbo.Invoices", "BillingAddress_Postcode", c => c.String(maxLength: 128));

            this.DeleteDefaultContstraint("dbo.InvoiceLineItems", "Name");
            AlterColumn("dbo.InvoiceLineItems", "Name", c => c.String(maxLength: 1024));

            this.DeleteDefaultContstraint("dbo.InvoiceLineItems", "Category");
            AlterColumn("dbo.InvoiceLineItems", "Category", c => c.String(maxLength: 1024));

            this.DeleteDefaultContstraint("dbo.InvoiceLineItems", "SubTitle");
            AlterColumn("dbo.InvoiceLineItems", "SubTitle", c => c.String(maxLength: 1024));

            this.DeleteDefaultContstraint("dbo.Mailshots", "Name");
            AlterColumn("dbo.Mailshots", "Name", c => c.String(nullable: false, maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Mailshots", "ProofPdfBlobId");
            AlterColumn("dbo.Mailshots", "ProofPdfBlobId", c => c.String(maxLength: 2048));

            this.DeleteDefaultContstraint("dbo.Mailshots", "ProofPdfUrl");
            AlterColumn("dbo.Mailshots", "ProofPdfUrl", c => c.String(maxLength: 2048));

            this.DeleteDefaultContstraint("dbo.Formats", "Name");
            AlterColumn("dbo.Formats", "Name", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Templates", "Name");
            AlterColumn("dbo.Templates", "Name", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.Themes", "Name");
            AlterColumn("dbo.Themes", "Name", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.PostalOptions", "Name");
            AlterColumn("dbo.PostalOptions", "Name", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.DistributionLists", "Name");
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.CmsImages", "Src");
            AlterColumn("dbo.CmsImages", "Src", c => c.String(nullable: false, maxLength: 2048));

            this.DeleteDefaultContstraint("dbo.CmsImages", "UserName");
            AlterColumn("dbo.CmsImages", "UserName", c => c.String(maxLength: 256));

            this.DeleteDefaultContstraint("dbo.MailshotDefaultContent", "Name");
            AlterColumn("dbo.MailshotDefaultContent", "Name", c => c.String(maxLength: 256));

            CreateIndex("dbo.Invoices", "OrderNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Invoices", new[] { "OrderNumber" });
            AlterColumn("dbo.MailshotDefaultContent", "Name", c => c.String());
            AlterColumn("dbo.CmsImages", "UserName", c => c.String());
            AlterColumn("dbo.CmsImages", "Src", c => c.String(nullable: false));
            AlterColumn("dbo.DistributionLists", "Name", c => c.String());
            AlterColumn("dbo.PostalOptions", "Name", c => c.String());
            AlterColumn("dbo.Themes", "Name", c => c.String());
            AlterColumn("dbo.Templates", "Name", c => c.String());
            AlterColumn("dbo.Formats", "Name", c => c.String());
            AlterColumn("dbo.Mailshots", "ProofPdfUrl", c => c.String());
            AlterColumn("dbo.Mailshots", "ProofPdfBlobId", c => c.String());
            AlterColumn("dbo.Mailshots", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.InvoiceLineItems", "SubTitle", c => c.String());
            AlterColumn("dbo.InvoiceLineItems", "Category", c => c.String());
            AlterColumn("dbo.InvoiceLineItems", "Name", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_Postcode", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_Country", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_City", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_Address2", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_Address1", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_BuildingName", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_BuildingNumber", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_FlatNumber", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_LastName", c => c.String());
            AlterColumn("dbo.Invoices", "BillingAddress_FirstName", c => c.String());
            AlterColumn("dbo.Invoices", "PaypalApprovalUrl", c => c.String());
            AlterColumn("dbo.Invoices", "PaypalPaymentId", c => c.String());
            AlterColumn("dbo.Invoices", "PaypalOrderId", c => c.String());
            AlterColumn("dbo.DataSearches", "ThirdPartyIdentifier", c => c.String());
            AlterColumn("dbo.DataSearches", "SearchCriteria", c => c.String());
            AlterColumn("dbo.DataSearches", "Name", c => c.String());
            AlterColumn("dbo.Campaigns", "SystemNotes", c => c.String());
            AlterColumn("dbo.Campaigns", "Notes", c => c.String());
            AlterColumn("dbo.Campaigns", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Invoices", "OrderNumber");
        }
    }
}
