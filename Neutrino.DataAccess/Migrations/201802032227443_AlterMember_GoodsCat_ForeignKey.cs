namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMember_GoodsCat_ForeignKey : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GoodsCategory", new[] { "GoodsCategoryType_Id" });
            DropIndex("dbo.Member", new[] { "PositionType_eId" });
            RenameColumn(table: "dbo.GoodsCategory", name: "GoodsCategoryType_Id", newName: "GoodsCatgeoryTypeId");
            RenameColumn(table: "dbo.Member", name: "PositionType_eId", newName: "PositionTypeId");
            AlterColumn("dbo.GoodsCategory", "GoodsCatgeoryTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Member", "PositionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoodsCategory", "GoodsCatgeoryTypeId");
            CreateIndex("dbo.Member", "PositionTypeId");
            DropColumn("dbo.GoodsCategory", "GoodsCatgoryTypeId");
            DropColumn("dbo.Member", "PositionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Member", "PositionId", c => c.Int(nullable: false));
            AddColumn("dbo.GoodsCategory", "GoodsCatgoryTypeId", c => c.Int(nullable: false));
            DropIndex("dbo.Member", new[] { "PositionTypeId" });
            DropIndex("dbo.GoodsCategory", new[] { "GoodsCatgeoryTypeId" });
            AlterColumn("dbo.Member", "PositionTypeId", c => c.Int());
            AlterColumn("dbo.GoodsCategory", "GoodsCatgeoryTypeId", c => c.Int());
            RenameColumn(table: "dbo.Member", name: "PositionTypeId", newName: "PositionType_eId");
            RenameColumn(table: "dbo.GoodsCategory", name: "GoodsCatgeoryTypeId", newName: "GoodsCategoryType_Id");
            CreateIndex("dbo.Member", "PositionType_eId");
            CreateIndex("dbo.GoodsCategory", "GoodsCategoryType_Id");
        }
    }
}
