namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeMailshotOptional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots");
            DropIndex("dbo.Campaigns", new[] { "MailshotId" });
            AlterColumn("dbo.Campaigns", "MailshotId", c => c.Guid());
            CreateIndex("dbo.Campaigns", "MailshotId");
            AddForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots", "MailshotId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots");
            DropIndex("dbo.Campaigns", new[] { "MailshotId" });
            AlterColumn("dbo.Campaigns", "MailshotId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Campaigns", "MailshotId");
            AddForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots", "MailshotId", cascadeDelete: true);
        }
    }
}
