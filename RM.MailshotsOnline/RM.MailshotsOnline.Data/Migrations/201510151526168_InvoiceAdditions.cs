namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceAdditions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "PaypalCaptureTransactionId", c => c.String(maxLength: 64));
            AddColumn("dbo.Invoices", "PaypalRefundTransactionId", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "PaypalRefundTransactionId");
            DropColumn("dbo.Invoices", "PaypalCaptureTransactionId");
        }
    }
}
