namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCmsImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmsImages",
                c => new
                    {
                        CmsImageId = c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Src = c.String(nullable: false),
                        UmbracoMediaId = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.CmsImageId);
            
            CreateTable(
                "dbo.MailshotImageUses",
                c => new
                    {
                        MailshotImageUseId = c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"),
                        CmsImageId = c.Guid(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        MailshotId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MailshotImageUseId)
                .ForeignKey("dbo.CmsImages", t => t.CmsImageId, cascadeDelete: true)
                .ForeignKey("dbo.Mailshots", t => t.MailshotId, cascadeDelete: true)
                .Index(t => t.CmsImageId)
                .Index(t => t.MailshotId);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MailshotImageUses", "MailshotId", "dbo.Mailshots");
            DropForeignKey("dbo.MailshotImageUses", "CmsImageId", "dbo.CmsImages");
            DropIndex("dbo.MailshotImageUses", new[] { "MailshotId" });
            DropIndex("dbo.MailshotImageUses", new[] { "CmsImageId" });
            DropTable("dbo.MailshotImageUses");
            DropTable("dbo.CmsImages");
        }
    }
}
