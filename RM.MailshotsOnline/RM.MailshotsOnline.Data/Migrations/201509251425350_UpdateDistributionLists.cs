namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDistributionLists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistributionLists", "BlobFinal", c => c.String());
            AddColumn("dbo.DistributionLists", "BlobWorking", c => c.String());
            AddColumn("dbo.DistributionLists", "BlobErrors", c => c.String());
            AddColumn("dbo.DistributionLists", "UpdatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.DistributionLists", "DataSalt", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistributionLists", "DataSalt");
            DropColumn("dbo.DistributionLists", "UpdatedDate");
            DropColumn("dbo.DistributionLists", "BlobErrors");
            DropColumn("dbo.DistributionLists", "BlobWorking");
            DropColumn("dbo.DistributionLists", "BlobFinal");
        }
    }
}
