namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branch", "RefId", c => c.Int(nullable: false));
            AddColumn("dbo.Goods", "RefId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Goods", "RefId");
            DropColumn("dbo.Branch", "RefId");
        }
    }
}
