namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDataSync : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataSyncStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(maxLength: 50),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        ReadedCount = c.Int(nullable: false),
                        InsertedCount = c.Int(nullable: false),
                        FailCount = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataSyncStatus");
        }
    }
}
