namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveOrders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Order", new[] { "CampaignId" });
            AddColumn("dbo.Invoices", "InvoicePdfBlobReference", c => c.String());
            AddColumn("dbo.Invoices", "PaidDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Invoices", "CancelledDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            DropTable("dbo.Order");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Guid(nullable: false, identity: true),
                        CampaignId = c.Guid(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CompletionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.OrderId);
            
            DropColumn("dbo.Invoices", "CancelledDate");
            DropColumn("dbo.Invoices", "PaidDate");
            DropColumn("dbo.Invoices", "InvoicePdfBlobReference");
            CreateIndex("dbo.Order", "CampaignId");
            AddForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns", "CampaignId", cascadeDelete: true);
        }
    }
}
