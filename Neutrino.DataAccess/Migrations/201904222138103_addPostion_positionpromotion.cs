namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPostion_positionpromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PositionReceiptPromotion", "PositionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.PositionReceiptPromotion", "PositionTypeId");
            AddForeignKey("dbo.PositionReceiptPromotion", "PositionTypeId", "dbo.PositionType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PositionReceiptPromotion", "PositionTypeId", "dbo.PositionType");
            DropIndex("dbo.PositionReceiptPromotion", new[] { "PositionTypeId" });
            DropColumn("dbo.PositionReceiptPromotion", "PositionTypeId");
        }
    }
}
