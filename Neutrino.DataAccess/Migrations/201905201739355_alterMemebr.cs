namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterMemebr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "RefPositionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "RefPositionId");
        }
    }
}
