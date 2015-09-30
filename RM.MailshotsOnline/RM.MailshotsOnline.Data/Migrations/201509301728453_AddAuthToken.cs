namespace RM.MailshotsOnline.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthToken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        AuthTokenId = c.Guid(nullable: false, identity: true, defaultValueSql: "newId()"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ServiceName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.AuthTokenId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuthTokens");
        }
    }
}
