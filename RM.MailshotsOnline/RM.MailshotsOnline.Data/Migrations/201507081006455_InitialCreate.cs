namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        CampaignId = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        FormatId = c.Int(nullable: false),
                        TemplateId = c.Int(nullable: false),
                        Content = c.String(),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CampaignId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Guid(nullable: false),
                        DistributionListId = c.Guid(nullable: false),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Address3 = c.String(),
                        Postcode = c.String(),
                        Country = c.String(),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.DistributionLists", t => t.DistributionListId, cascadeDelete: true)
                .Index(t => t.DistributionListId);
            
            CreateTable(
                "dbo.DistributionLists",
                c => new
                    {
                        DistributionListId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Order_OrderId = c.Guid(),
                    })
                .PrimaryKey(t => t.DistributionListId)
                .ForeignKey("dbo.Order", t => t.Order_OrderId)
                .Index(t => t.Order_OrderId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Guid(nullable: false),
                        CampaignId = c.Guid(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CompletionDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Contacts", "DistributionListId", "dbo.DistributionLists");
            DropIndex("dbo.Order", new[] { "CampaignId" });
            DropIndex("dbo.DistributionLists", new[] { "Order_OrderId" });
            DropIndex("dbo.Contacts", new[] { "DistributionListId" });
            DropTable("dbo.Order");
            DropTable("dbo.DistributionLists");
            DropTable("dbo.Contacts");
            DropTable("dbo.Campaigns");
        }
    }
}
