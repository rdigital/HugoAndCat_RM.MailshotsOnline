namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            DropPrimaryKey("dbo.MailshotContents");
            AlterColumn("dbo.MailshotContents", "MailshotContentId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AddPrimaryKey("dbo.MailshotContents", "MailshotContentId");
            AddForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents", "MailshotContentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            DropPrimaryKey("dbo.MailshotContents");
            AlterColumn("dbo.MailshotContents", "MailshotContentId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.MailshotContents", "MailshotContentId");
            AddForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents", "MailshotContentId", cascadeDelete: true);
        }
    }
}
