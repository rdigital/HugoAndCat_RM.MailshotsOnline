namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayPalToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "PaypalToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "PaypalToken");
        }
    }
}
