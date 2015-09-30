namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressesToInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "BillingAddress_FirstName", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_LastName", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_FlatNumber", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_BuildingNumber", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_BuildingName", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_Address1", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_Address2", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_City", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_Country", c => c.String());
            AddColumn("dbo.Invoices", "BillingAddress_Postcode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "BillingAddress_Postcode");
            DropColumn("dbo.Invoices", "BillingAddress_Country");
            DropColumn("dbo.Invoices", "BillingAddress_City");
            DropColumn("dbo.Invoices", "BillingAddress_Address2");
            DropColumn("dbo.Invoices", "BillingAddress_Address1");
            DropColumn("dbo.Invoices", "BillingAddress_BuildingName");
            DropColumn("dbo.Invoices", "BillingAddress_BuildingNumber");
            DropColumn("dbo.Invoices", "BillingAddress_FlatNumber");
            DropColumn("dbo.Invoices", "BillingAddress_LastName");
            DropColumn("dbo.Invoices", "BillingAddress_FirstName");
        }
    }
}
