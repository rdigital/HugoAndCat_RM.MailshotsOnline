namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormatPriceUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formats", "PricePerPrint", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Formats", "OnceOffPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Formats", "OnceOffPrice");
            DropColumn("dbo.Formats", "PricePerPrint");
        }
    }
}
