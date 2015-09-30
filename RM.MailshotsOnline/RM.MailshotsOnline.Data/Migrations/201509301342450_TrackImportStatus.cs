namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackImportStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistributionLists", "ListState", c => c.String(maxLength: 20));
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 256));
            DropColumn("dbo.DistributionLists", "ListState");
        }
    }
}
