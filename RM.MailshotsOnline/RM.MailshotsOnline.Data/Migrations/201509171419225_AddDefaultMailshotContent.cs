namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultMailshotContent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailshotDefaultContent",
                c => new
                    {
                        MailshotDefaultContentId = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        JsonIndex = c.Int(nullable: false),
                        JsonData = c.String(),
                        UmbracoPageId = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.MailshotDefaultContentId)
                .Index(t => t.JsonIndex)
                .Index(t => t.UmbracoPageId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.MailshotDefaultContent", new[] { "UmbracoPageId" });
            DropIndex("dbo.MailshotDefaultContent", new[] { "JsonIndex" });
            DropTable("dbo.MailshotDefaultContent");
        }
    }
}
