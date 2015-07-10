namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "SerialisedData", c => c.String());
            DropColumn("dbo.Contacts", "Name");
            DropColumn("dbo.Contacts", "Address1");
            DropColumn("dbo.Contacts", "Address2");
            DropColumn("dbo.Contacts", "Address3");
            DropColumn("dbo.Contacts", "Postcode");
            DropColumn("dbo.Contacts", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Country", c => c.String());
            AddColumn("dbo.Contacts", "Postcode", c => c.String());
            AddColumn("dbo.Contacts", "Address3", c => c.String());
            AddColumn("dbo.Contacts", "Address2", c => c.String());
            AddColumn("dbo.Contacts", "Address1", c => c.String());
            AddColumn("dbo.Contacts", "Name", c => c.String());
            DropColumn("dbo.Contacts", "SerialisedData");
        }
    }
}
