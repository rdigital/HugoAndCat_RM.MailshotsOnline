namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DistributionListRecordCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistributionLists", "RecordCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistributionLists", "RecordCount");
        }
    }
}
