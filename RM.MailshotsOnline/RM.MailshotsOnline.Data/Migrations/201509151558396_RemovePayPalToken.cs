namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePayPalToken : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Invoices", "PaypalToken");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "PaypalToken", c => c.String());
        }
    }
}
