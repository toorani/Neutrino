namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefIdToMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "RefId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "RefId");
        }
    }
}
