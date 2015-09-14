namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceLines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceLineItems",
                c => new
                    {
                        InvoiceLineItemId = c.Guid(nullable: false, identity: true),
                        InvoiceId = c.Guid(nullable: false),
                        Name = c.String(),
                        Quantity = c.Int(nullable: false),
                        UnitCost = c.Decimal(nullable: false, storeType: "money"),
                        SubTotal = c.Decimal(nullable: false, storeType: "money"),
                        TaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, storeType: "money"),
                        Total = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.InvoiceLineItemId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .Index(t => t.InvoiceId);
            
            DropColumn("dbo.Invoices", "DataRentalCost");
            DropColumn("dbo.Invoices", "DataRentalCount");
            DropColumn("dbo.Invoices", "DataRentalFlatFee");
            DropColumn("dbo.Invoices", "DataRentalRate");
            DropColumn("dbo.Invoices", "PostageCost");
            DropColumn("dbo.Invoices", "PostageRate");
            DropColumn("dbo.Invoices", "PrintCount");
            DropColumn("dbo.Invoices", "PrintingCost");
            DropColumn("dbo.Invoices", "PrintingRate");
            DropColumn("dbo.Invoices", "ServiceFee");
            DropColumn("dbo.Invoices", "TaxRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "TaxRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "ServiceFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "PrintingRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "PrintingCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "PrintCount", c => c.Int(nullable: false));
            AddColumn("dbo.Invoices", "PostageRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "PostageCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "DataRentalRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "DataRentalFlatFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "DataRentalCount", c => c.Int(nullable: false));
            AddColumn("dbo.Invoices", "DataRentalCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.InvoiceLineItems", "InvoiceId", "dbo.Invoices");
            DropIndex("dbo.InvoiceLineItems", new[] { "InvoiceId" });
            DropTable("dbo.InvoiceLineItems");
        }
    }
}
