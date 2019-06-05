namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRelationElitePosition : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PositionMapping", "ElitePositionId");
            AddForeignKey("dbo.PositionMapping", "ElitePositionId", "dbo.ElitePosition", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PositionMapping", "ElitePositionId", "dbo.ElitePosition");
            DropIndex("dbo.PositionMapping", new[] { "ElitePositionId" });
        }
    }
}
