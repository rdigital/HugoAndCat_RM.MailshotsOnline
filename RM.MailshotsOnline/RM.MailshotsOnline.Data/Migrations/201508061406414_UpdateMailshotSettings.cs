namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMailshotSettings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Templates", "FormatId", "dbo.Formats");
            DropIndex("dbo.Templates", new[] { "FormatId" });
            AddColumn("dbo.Templates", "FormatUmbracoPageId", c => c.Int(nullable: false));
            DropColumn("dbo.Mailshots", "FormatId");
            DropColumn("dbo.Mailshots", "TemplateId");
            DropColumn("dbo.Mailshots", "ThemeId");
            AddColumn("dbo.Mailshots", "FormatId", c => c.Guid(nullable: false, defaultValue: Guid.Empty));
            AddColumn("dbo.Mailshots", "TemplateId", c => c.Guid(nullable: false, defaultValue: Guid.Empty));
            AddColumn("dbo.Mailshots", "ThemeId", c => c.Guid(nullable: false, defaultValue: Guid.Empty));
            CreateIndex("dbo.Mailshots", "FormatId");
            CreateIndex("dbo.Mailshots", "TemplateId");
            CreateIndex("dbo.Mailshots", "ThemeId");
            CreateIndex("dbo.Templates", "FormatUmbracoPageId");
            AddForeignKey("dbo.Mailshots", "FormatId", "dbo.Formats", "FormatId", cascadeDelete: true);
            AddForeignKey("dbo.Mailshots", "TemplateId", "dbo.Templates", "TemplateId", cascadeDelete: true);
            AddForeignKey("dbo.Mailshots", "ThemeId", "dbo.Themes", "ThemeId", cascadeDelete: true);
            DropColumn("dbo.Templates", "FormatId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Templates", "FormatId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Mailshots", "ThemeId", "dbo.Themes");
            DropForeignKey("dbo.Mailshots", "TemplateId", "dbo.Templates");
            DropForeignKey("dbo.Mailshots", "FormatId", "dbo.Formats");
            DropIndex("dbo.Templates", new[] { "FormatUmbracoPageId" });
            DropIndex("dbo.Mailshots", new[] { "ThemeId" });
            DropIndex("dbo.Mailshots", new[] { "TemplateId" });
            DropIndex("dbo.Mailshots", new[] { "FormatId" });
            DropColumn("dbo.Mailshots", "FormatId");
            DropColumn("dbo.Mailshots", "TemplateId");
            DropColumn("dbo.Mailshots", "ThemeId");
            AddColumn("dbo.Mailshots", "ThemeId", c => c.Int(nullable: false));
            AddColumn("dbo.Mailshots", "TemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.Mailshots", "FormatId", c => c.Int(nullable: false));
            DropColumn("dbo.Templates", "FormatUmbracoPageId");
            CreateIndex("dbo.Templates", "FormatId");
            AddForeignKey("dbo.Templates", "FormatId", "dbo.Formats", "FormatId", cascadeDelete: true);
        }
    }
}
