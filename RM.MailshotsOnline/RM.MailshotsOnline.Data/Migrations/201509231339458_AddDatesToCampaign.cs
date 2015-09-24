namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatesToCampaign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Campaigns", "OrderPlacedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Campaigns", "CancelledDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Campaigns", "OrderDespatchedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));

            // Using this opportunity to add newid() to primary keys
            AlterColumn("dbo.DataSearches", "DataSearchId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Formats", "FormatId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.InvoiceLineItems", "InvoiceLineItemId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Invoices", "InvoiceId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.MailshotDefaultContent", "MailshotDefaultContentId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.SettingsFromCms", "SettingsId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Templates", "TemplateId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Themes", "ThemeId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Campaigns", "OrderDespatchedDate");
            DropColumn("dbo.Campaigns", "CancelledDate");
            DropColumn("dbo.Campaigns", "OrderPlacedDate");
        }
    }
}
