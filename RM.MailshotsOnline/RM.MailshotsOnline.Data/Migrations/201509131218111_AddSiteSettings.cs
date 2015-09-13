namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSiteSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SettingsFromCms",
                c => new
                    {
                        SettingsId = c.Guid(nullable: false, identity: true),
                        MsolPerUseFee = c.Decimal(nullable: false, storeType: "money"),
                        VatRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePerRentedDataRecord = c.Decimal(nullable: false, storeType: "money"),
                        DataRentalServiceFee = c.Decimal(nullable: false, storeType: "money"),
                        UmbracoContentId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.SettingsId);
            
            DropColumn("dbo.Formats", "OnceOffPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Formats", "OnceOffPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.SettingsFromCms");
        }
    }
}
