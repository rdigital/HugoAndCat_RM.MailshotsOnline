namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMailshots : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mailshots",
                c => new
                    {
                        MailshotId = c.Guid(nullable: false),
                        MailshotContentId = c.Guid(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Draft = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MailshotId)
                .ForeignKey("dbo.MailshotContents", t => t.MailshotContentId, cascadeDelete: true)
                .Index(t => t.MailshotContentId);
            
            CreateTable(
                "dbo.MailshotContents",
                c => new
                    {
                        MailshotContentId = c.Guid(nullable: false),
                        Content = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.MailshotContentId);
            
            AddColumn("dbo.Campaigns", "MailshotId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Contacts", "SerialisedData", c => c.String(unicode: false, storeType: "text"));
            CreateIndex("dbo.Campaigns", "MailshotId");
            AddForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots", "MailshotId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Campaigns", "MailshotId", "dbo.Mailshots");
            DropForeignKey("dbo.Mailshots", "MailshotContentId", "dbo.MailshotContents");
            DropIndex("dbo.Mailshots", new[] { "MailshotContentId" });
            DropIndex("dbo.Campaigns", new[] { "MailshotId" });
            AlterColumn("dbo.Contacts", "SerialisedData", c => c.String());
            DropColumn("dbo.Campaigns", "MailshotId");
            DropTable("dbo.MailshotContents");
            DropTable("dbo.Mailshots");
        }
    }
}
