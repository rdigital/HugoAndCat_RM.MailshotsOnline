namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMailshotSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Formats",
                c => new
                    {
                        FormatId = c.Guid(nullable: false, identity: true),
                        JsonIndex = c.Int(nullable: false),
                        Name = c.String(),
                        UmbracoPageId = c.Int(nullable: false),
                        XslData = c.String(),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.FormatId)
                .Index(t => t.JsonIndex)
                .Index(t => t.UmbracoPageId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        TemplateId = c.Guid(nullable: false, identity: true),
                        JsonIndex = c.Int(nullable: false),
                        Name = c.String(),
                        FormatId = c.Guid(nullable: false),
                        UmbracoPageId = c.Int(nullable: false),
                        XslData = c.String(),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.TemplateId)
                .ForeignKey("dbo.Formats", t => t.FormatId, cascadeDelete: true)
                .Index(t => t.JsonIndex)
                .Index(t => t.FormatId)
                .Index(t => t.UmbracoPageId);
            
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        ThemeId = c.Guid(nullable: false, identity: true),
                        JsonIndex = c.Int(nullable: false),
                        Name = c.String(),
                        UmbracoPageId = c.Int(nullable: false),
                        XslData = c.String(),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ThemeId)
                .Index(t => t.JsonIndex)
                .Index(t => t.UmbracoPageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Templates", "FormatId", "dbo.Formats");
            DropIndex("dbo.Themes", new[] { "UmbracoPageId" });
            DropIndex("dbo.Themes", new[] { "JsonIndex" });
            DropIndex("dbo.Templates", new[] { "UmbracoPageId" });
            DropIndex("dbo.Templates", new[] { "FormatId" });
            DropIndex("dbo.Templates", new[] { "JsonIndex" });
            DropIndex("dbo.Formats", new[] { "UmbracoPageId" });
            DropIndex("dbo.Formats", new[] { "JsonIndex" });
            DropTable("dbo.Themes");
            DropTable("dbo.Templates");
            DropTable("dbo.Formats");
        }
    }
}
