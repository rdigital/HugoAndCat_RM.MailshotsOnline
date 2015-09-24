namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublicHolidayDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingsFromCms", "PublicHolidays", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SettingsFromCms", "PublicHolidays");
        }
    }
}
