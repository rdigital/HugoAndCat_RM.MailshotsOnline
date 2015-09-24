namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEstimatesToSettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingsFromCms", "ModerationTimeEstimate", c => c.Int(nullable: false));
            AddColumn("dbo.SettingsFromCms", "PrintingTimeEstimate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SettingsFromCms", "PrintingTimeEstimate");
            DropColumn("dbo.SettingsFromCms", "ModerationTimeEstimate");
        }
    }
}
