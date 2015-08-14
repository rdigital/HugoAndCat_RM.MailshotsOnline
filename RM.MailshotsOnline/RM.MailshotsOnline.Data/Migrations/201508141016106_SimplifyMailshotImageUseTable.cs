namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifyMailshotImageUseTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MailshotImageUses");
            AddPrimaryKey("dbo.MailshotImageUses", new[] { "CmsImageId", "MailshotId" });
            DropColumn("dbo.MailshotImageUses", "MailshotImageUseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MailshotImageUses", "MailshotImageUseId", c => c.Guid(nullable: false, identity: true));
            DropPrimaryKey("dbo.MailshotImageUses");
            AddPrimaryKey("dbo.MailshotImageUses", "MailshotImageUseId");
        }
    }
}
