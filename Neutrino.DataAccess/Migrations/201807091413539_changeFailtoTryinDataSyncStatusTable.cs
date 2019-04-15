namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFailtoTryinDataSyncStatusTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataSyncStatus", "TryCount", c => c.Int(nullable: false));
            DropColumn("dbo.DataSyncStatus", "FailCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataSyncStatus", "FailCount", c => c.Int(nullable: false));
            DropColumn("dbo.DataSyncStatus", "TryCount");
        }
    }
}
