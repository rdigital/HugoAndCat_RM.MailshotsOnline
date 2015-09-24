namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeliveryEstimates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostalOptions", "DeliveryTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostalOptions", "DeliveryTime");
        }
    }
}
