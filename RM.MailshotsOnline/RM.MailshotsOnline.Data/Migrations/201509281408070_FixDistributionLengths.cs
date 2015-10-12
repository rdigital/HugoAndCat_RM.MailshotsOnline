namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDistributionLengths : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.DistributionLists", "BlobFinal", c => c.String(maxLength: 500));
            AlterColumn("dbo.DistributionLists", "BlobWorking", c => c.String(maxLength: 500));
            AlterColumn("dbo.DistributionLists", "BlobErrors", c => c.String(maxLength: 500));
            AlterColumn("dbo.DistributionLists", "DataSalt", c => c.Binary(maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DistributionLists", "DataSalt", c => c.Binary());
            AlterColumn("dbo.DistributionLists", "BlobErrors", c => c.String());
            AlterColumn("dbo.DistributionLists", "BlobWorking", c => c.String());
            AlterColumn("dbo.DistributionLists", "BlobFinal", c => c.String());
            AlterColumn("dbo.DistributionLists", "Name", c => c.String());
        }
    }
}
