namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterPositionName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionMapping", "PositionRefId", c => c.Int(nullable: false));
            DropColumn("dbo.PositionMapping", "PostionRefId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PositionMapping", "PostionRefId", c => c.Int(nullable: false));
            DropColumn("dbo.PositionMapping", "PositionRefId");
        }
    }
}
