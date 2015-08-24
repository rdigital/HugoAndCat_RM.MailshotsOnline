namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DistributionListUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions");
            DropIndex("dbo.DistributionLists", new[] { "Order_OrderId" });
            DropIndex("dbo.Order", new[] { "PostalOptionId" });
            AddColumn("dbo.DistributionLists", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.DistributionLists", "Order_OrderId");
            DropColumn("dbo.Order", "PostalOptionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "PostalOptionId", c => c.Guid(nullable: false));
            AddColumn("dbo.DistributionLists", "Order_OrderId", c => c.Guid());
            DropColumn("dbo.DistributionLists", "UserId");
            CreateIndex("dbo.Order", "PostalOptionId");
            CreateIndex("dbo.DistributionLists", "Order_OrderId");
            AddForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions", "PostalOptionId", cascadeDelete: true);
            AddForeignKey("dbo.DistributionLists", "Order_OrderId", "dbo.Order", "OrderId");
        }
    }
}
