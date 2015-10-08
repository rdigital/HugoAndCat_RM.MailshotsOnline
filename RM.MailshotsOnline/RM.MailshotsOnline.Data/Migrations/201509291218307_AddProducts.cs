namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductSku = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 1024),
                        Category = c.String(maxLength: 1024),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ProductSku);
            
            AddColumn("dbo.InvoiceLineItems", "ProductSku", c => c.String(maxLength: 128));
            AlterColumn("dbo.InvoiceLineItems", "Name", c => c.String(maxLength: 1024));
            AlterColumn("dbo.InvoiceLineItems", "Category", c => c.String(maxLength: 1024));
            CreateIndex("dbo.InvoiceLineItems", "ProductSku");
            AddForeignKey("dbo.InvoiceLineItems", "ProductSku", "dbo.Products", "ProductSku");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceLineItems", "ProductSku", "dbo.Products");
            DropIndex("dbo.InvoiceLineItems", new[] { "ProductSku" });
            AlterColumn("dbo.InvoiceLineItems", "Category", c => c.String(maxLength: 1024));
            AlterColumn("dbo.InvoiceLineItems", "Name", c => c.String(maxLength: 1024));
            DropColumn("dbo.InvoiceLineItems", "ProductSku");
            DropTable("dbo.Products");
        }
    }
}
