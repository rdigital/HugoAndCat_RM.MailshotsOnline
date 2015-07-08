namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostalOption : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostalOptions",
                c => new
                    {
                        PostalOptionId = c.Guid(nullable: false),
                        FormatId = c.Int(nullable: false),
                        Name = c.String(),
                        Currency = c.String(maxLength: 3),
                        PricePerUnit = c.Decimal(nullable: false, storeType: "money"),
                        Tax = c.Decimal(nullable: false, storeType: "money"),
                        TaxCode = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.PostalOptionId);
            
            AddColumn("dbo.Order", "PostalOptionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Order", "PostalOptionId");
            AddForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions", "PostalOptionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "PostalOptionId", "dbo.PostalOptions");
            DropIndex("dbo.Order", new[] { "PostalOptionId" });
            DropColumn("dbo.Order", "PostalOptionId");
            DropTable("dbo.PostalOptions");
        }
    }
}
