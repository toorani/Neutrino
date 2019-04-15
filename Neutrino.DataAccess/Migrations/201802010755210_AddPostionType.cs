namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostionType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PositionType", "Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PositionType", "Id1", c => c.Int(nullable: false));
        }
    }
}
