namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablepostiontype_member : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Member", new[] { "PositionTypeId" });
            AlterColumn("dbo.Member", "PositionTypeId", c => c.Int());
            CreateIndex("dbo.Member", "PositionTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Member", new[] { "PositionTypeId" });
            AlterColumn("dbo.Member", "PositionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Member", "PositionTypeId");
        }
    }
}
