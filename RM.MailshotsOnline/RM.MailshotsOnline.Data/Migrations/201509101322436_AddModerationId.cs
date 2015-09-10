namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModerationId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "ModerationId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Campaigns", "ModerationId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Campaigns", new[] { "ModerationId" });
            DropColumn("dbo.Campaigns", "ModerationId");
        }
    }
}
