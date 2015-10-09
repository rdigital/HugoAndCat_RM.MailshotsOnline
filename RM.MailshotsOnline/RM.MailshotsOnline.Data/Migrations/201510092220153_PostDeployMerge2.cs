namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostDeployMerge2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DistributionLists", "BlobFinal", c => c.String(maxLength: 500));
            AlterColumn("dbo.DistributionLists", "BlobWorking", c => c.String(maxLength: 500));
            AlterColumn("dbo.DistributionLists", "BlobErrors", c => c.String(maxLength: 500));
            AlterColumn("dbo.DistributionLists", "UpdatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.DistributionLists", "DataSalt", c => c.Binary(maxLength: 32));
            AlterColumn("dbo.DistributionLists", "ListState", c => c.String(maxLength: 20));
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 256));
        }
    }
}
