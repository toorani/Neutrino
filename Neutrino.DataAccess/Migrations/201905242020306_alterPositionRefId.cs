namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterPositionRefId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "PositionRefId", c => c.Int(nullable: false));
            DropColumn("dbo.Member", "RefPositionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Member", "RefPositionId", c => c.Int(nullable: false));
            DropColumn("dbo.Member", "PositionRefId");
        }
    }
}
