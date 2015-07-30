namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MailshotsUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mailshots", "FormatId", c => c.Int(nullable: false));
            AddColumn("dbo.Mailshots", "TemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.Mailshots", "ThemeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Mailshots", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mailshots", "Name", c => c.String());
            DropColumn("dbo.Mailshots", "ThemeId");
            DropColumn("dbo.Mailshots", "TemplateId");
            DropColumn("dbo.Mailshots", "FormatId");
        }
    }
}
