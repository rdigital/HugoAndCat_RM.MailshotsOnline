namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostDevelopMerge : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DistributionLists", "Name", c => c.String(maxLength: 100));
        }
    }
}
