namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMailshotSettings1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Formats", "JsonData", c => c.String());
            AddColumn("dbo.Templates", "JsonData", c => c.String());
            AddColumn("dbo.Themes", "JsonData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Themes", "JsonData");
            DropColumn("dbo.Templates", "JsonData");
            DropColumn("dbo.Formats", "JsonData");
        }
    }
}
