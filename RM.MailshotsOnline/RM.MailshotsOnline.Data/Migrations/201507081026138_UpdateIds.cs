namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns");
            DropForeignKey("dbo.Contacts", "DistributionListId", "dbo.DistributionLists");
            DropForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions");
            DropPrimaryKey("dbo.Campaigns");
            DropPrimaryKey("dbo.Contacts");
            DropPrimaryKey("dbo.DistributionLists");
            DropPrimaryKey("dbo.Order");
            DropPrimaryKey("dbo.PostalOptions");
            AlterColumn("dbo.Campaigns", "CampaignId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Contacts", "ContactId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.DistributionLists", "DistributionListId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Order", "OrderId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.PostalOptions", "PostalOptionId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AddPrimaryKey("dbo.Campaigns", "CampaignId");
            AddPrimaryKey("dbo.Contacts", "ContactId");
            AddPrimaryKey("dbo.DistributionLists", "DistributionListId");
            AddPrimaryKey("dbo.Order", "OrderId");
            AddPrimaryKey("dbo.PostalOptions", "PostalOptionId");
            AddForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns", "CampaignId", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "DistributionListId", "dbo.DistributionLists", "DistributionListId", cascadeDelete: true);
            AddForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order", "OrderId");
            AddForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions", "PostalOptionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions");
            DropForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order");
            DropForeignKey("dbo.Contacts", "DistributionListId", "dbo.DistributionLists");
            DropForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns");
            DropPrimaryKey("dbo.PostalOptions");
            DropPrimaryKey("dbo.Order");
            DropPrimaryKey("dbo.DistributionLists");
            DropPrimaryKey("dbo.Contacts");
            DropPrimaryKey("dbo.Campaigns");
            AlterColumn("dbo.PostalOptions", "PostalOptionId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Order", "OrderId", c => c.Guid(nullable: false));
            AlterColumn("dbo.DistributionLists", "DistributionListId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Contacts", "ContactId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Campaigns", "CampaignId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.PostalOptions", "PostalOptionId");
            AddPrimaryKey("dbo.Order", "OrderId");
            AddPrimaryKey("dbo.DistributionLists", "DistributionListId");
            AddPrimaryKey("dbo.Contacts", "ContactId");
            AddPrimaryKey("dbo.Campaigns", "CampaignId");
            AddForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions", "PostalOptionId", cascadeDelete: true);
            AddForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order", "OrderId");
            AddForeignKey("dbo.Contacts", "DistributionListId", "dbo.DistributionLists", "DistributionListId", cascadeDelete: true);
            AddForeignKey("dbo.Order", "CampaignId", "dbo.Campaigns", "CampaignId", cascadeDelete: true);
        }
    }
}
