namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackImportStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistributionLists", "ListState", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistributionLists", "ListState");
        }
    }
}
