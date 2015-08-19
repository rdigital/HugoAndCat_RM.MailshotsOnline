namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignsUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CampaignDistributionLists",
                c => new
                    {
                        CampaignId = c.Guid(nullable: false),
                        DistributionListId = c.Guid(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => new { t.CampaignId, t.DistributionListId })
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .ForeignKey("dbo.DistributionLists", t => t.DistributionListId, cascadeDelete: true)
                .Index(t => t.CampaignId)
                .Index(t => t.DistributionListId);
            
            CreateTable(
                "dbo.DataSearches",
                c => new
                    {
                        DataSearchId = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        SearchCriteria = c.String(),
                        ThirdPartyIdentifier = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CampaignId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.DataSearchId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
            AddColumn("dbo.Campaigns", "UpdatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Campaigns", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Campaigns", "Notes", c => c.String());
            AddColumn("dbo.Campaigns", "SystemNotes", c => c.String());
            CreateIndex("dbo.Campaigns", "UserId");
            DropColumn("dbo.Campaigns", "FormatId");
            DropColumn("dbo.Campaigns", "TemplateId");
            DropColumn("dbo.Campaigns", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Campaigns", "Content", c => c.String());
            AddColumn("dbo.Campaigns", "TemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.Campaigns", "FormatId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CampaignDistributionLists", "DistributionListId", "dbo.DistributionLists");
            DropForeignKey("dbo.DataSearches", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignDistributionLists", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.DataSearches", new[] { "CampaignId" });
            DropIndex("dbo.Campaigns", new[] { "UserId" });
            DropIndex("dbo.CampaignDistributionLists", new[] { "DistributionListId" });
            DropIndex("dbo.CampaignDistributionLists", new[] { "CampaignId" });
            DropColumn("dbo.Campaigns", "SystemNotes");
            DropColumn("dbo.Campaigns", "Notes");
            DropColumn("dbo.Campaigns", "Name");
            DropColumn("dbo.Campaigns", "UpdatedDate");
            DropTable("dbo.DataSearches");
            DropTable("dbo.CampaignDistributionLists");
        }
    }
}
