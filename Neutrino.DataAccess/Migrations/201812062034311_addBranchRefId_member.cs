namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBranchRefId_member : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "BranchRefId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "BranchRefId");
        }
    }
}
