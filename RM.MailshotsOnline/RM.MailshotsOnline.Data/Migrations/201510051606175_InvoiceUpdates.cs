namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "BillingAddress_Title", c => c.String(maxLength: 256));
            AddColumn("dbo.Invoices", "BillingAddress_County", c => c.String(maxLength: 256));
            AddColumn("dbo.Invoices", "BillingEmail", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "BillingEmail");
            DropColumn("dbo.Invoices", "BillingAddress_County");
            DropColumn("dbo.Invoices", "BillingAddress_Title");
        }
    }
}
