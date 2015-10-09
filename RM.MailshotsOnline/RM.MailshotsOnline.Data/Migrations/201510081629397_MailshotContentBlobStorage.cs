namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MailshotContentBlobStorage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            DropIndex("dbo.Mailshots", new[] { "MailshotContentId" });
            AddColumn("dbo.Mailshots", "ContentBlobId", c => c.String(maxLength: 256));
            AlterColumn("dbo.Mailshots", "MailshotContentId", c => c.Guid());
            CreateIndex("dbo.Mailshots", "MailshotContentId");
            AddForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents", "MailshotContentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            DropIndex("dbo.Mailshots", new[] { "MailshotContentId" });
            AlterColumn("dbo.Mailshots", "MailshotContentId", c => c.Guid(nullable: false));
            DropColumn("dbo.Mailshots", "ContentBlobId");
            CreateIndex("dbo.Mailshots", "MailshotContentId");
            AddForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents", "MailshotContentId", cascadeDelete: true);
        }
    }
}
