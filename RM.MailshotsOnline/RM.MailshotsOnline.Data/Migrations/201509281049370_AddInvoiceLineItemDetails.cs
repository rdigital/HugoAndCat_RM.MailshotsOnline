namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceLineItemDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceLineItems", "Category", c => c.String());
            AddColumn("dbo.InvoiceLineItems", "SubTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceLineItems", "SubTitle");
            DropColumn("dbo.InvoiceLineItems", "Category");
        }
    }
}
