namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots");
            DropPrimaryKey("dbo.Mailshots");
            AlterColumn("dbo.Mailshots", "MailshotId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AddPrimaryKey("dbo.Mailshots", "MailshotId");
            AddForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots", "MailshotId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots");
            DropPrimaryKey("dbo.Mailshots");
            AlterColumn("dbo.Mailshots", "MailshotId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Mailshots", "MailshotId");
            AddForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots", "MailshotId", cascadeDelete: true);
        }
    }
}
