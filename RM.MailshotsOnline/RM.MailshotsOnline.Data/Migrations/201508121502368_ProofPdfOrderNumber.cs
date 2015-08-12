namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProofPdfOrderNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mailshots", "ProofPdfOrderNumber", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mailshots", "ProofPdfOrderNumber");
        }
    }
}
