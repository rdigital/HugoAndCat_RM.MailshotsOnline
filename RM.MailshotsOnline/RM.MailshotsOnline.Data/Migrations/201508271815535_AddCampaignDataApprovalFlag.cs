namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaignDataApprovalFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "DataSetsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "DataSetsApproved");
        }
    }
}
