namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PricingModelChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "PostalOptionId", c => c.Guid());
            AddColumn("dbo.DataSearches", "RecordCount", c => c.Int(nullable: false));
            AddColumn("dbo.PostalOptions", "UmbracoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Campaigns", "PostalOptionId");
            AddForeignKey("dbo.Campaigns", "PostalOptionId", "dbo.PostalOptions", "PostalOptionId");
            DropColumn("dbo.PostalOptions", "FormatId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PostalOptions", "FormatId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Campaigns", "PostalOptionId", "dbo.PostalOptions");
            DropIndex("dbo.Campaigns", new[] { "PostalOptionId" });
            DropColumn("dbo.PostalOptions", "UmbracoId");
            DropColumn("dbo.DataSearches", "RecordCount");
            DropColumn("dbo.Campaigns", "PostalOptionId");
        }
    }
}
