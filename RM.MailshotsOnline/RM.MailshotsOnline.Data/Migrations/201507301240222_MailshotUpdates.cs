namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MailshotUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mailshots", "ProofPdfBlobId", c => c.String());
            AddColumn("dbo.Mailshots", "ProofPdfUrl", c => c.String());
            AddColumn("dbo.Mailshots", "ProofPdfStatus", c => c.Int(nullable: false));
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            AddForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents", "MailshotContentId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mailshots", "ProofPdfStatus");
            DropColumn("dbo.Mailshots", "ProofPdfUrl");
            DropColumn("dbo.Mailshots", "ProofPdfBlobId");
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            AddForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents", "MailshotContentId", cascadeDelete: true);
        }
    }
}
