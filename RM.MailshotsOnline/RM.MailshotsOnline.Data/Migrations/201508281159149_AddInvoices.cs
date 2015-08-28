namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Guid(nullable: false, identity: true),
                        CampaignId = c.Guid(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Status = c.Int(nullable: false),
                        PaypalOrderId = c.String(),
                        PaypalPaymentId = c.String(),
                        DataRentalCount = c.Int(nullable: false),
                        DataRentalFlatFee = c.Decimal(nullable: false, storeType: "money"),
                        DataRentalRate = c.Decimal(nullable: false, storeType: "money"),
                        DataRentalCost = c.Decimal(nullable: false, storeType: "money"),
                        PostageRate = c.Decimal(nullable: false, storeType: "money"),
                        PostageCost = c.Decimal(nullable: false, storeType: "money"),
                        PrintCount = c.Int(nullable: false),
                        PrintingRate = c.Decimal(nullable: false, storeType: "money"),
                        PrintingCost = c.Decimal(nullable: false, storeType: "money"),
                        ServiceFee = c.Decimal(nullable: false, storeType: "money"),
                        SubTotal = c.Decimal(nullable: false, storeType: "money"),
                        TaxRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalTax = c.Decimal(nullable: false, storeType: "money"),
                        Total = c.Decimal(nullable: false, storeType: "money"),
                })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Campaigns", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "CampaignId", "dbo.Campaigns");
            DropIndex("dbo.Invoices", new[] { "CampaignId" });
            DropTable("dbo.Invoices");
        }
    }
}
