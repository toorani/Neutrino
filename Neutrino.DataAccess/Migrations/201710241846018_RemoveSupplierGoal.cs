namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSupplierGoal : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GoodsGoodsCategory", newName: "GoodsCategoryGoods");
            DropForeignKey("dbo.Goal", "SupplierGoalId", "dbo.Goal");
            DropIndex("dbo.Goal", new[] { "SupplierGoalId" });
            RenameColumn(table: "dbo.GoodsCategoryGoods", name: "Goods_Id", newName: "GoodsId");
            RenameColumn(table: "dbo.GoodsCategoryGoods", name: "GoodsCategory_Id", newName: "GoodsCategoryId");
            RenameIndex(table: "dbo.GoodsCategoryGoods", name: "IX_GoodsCategory_Id", newName: "IX_GoodsCategoryId");
            RenameIndex(table: "dbo.GoodsCategoryGoods", name: "IX_Goods_Id", newName: "IX_GoodsId");
            DropPrimaryKey("dbo.GoodsCategoryGoods");
            //Sql("ALTER TABLE dbo.GoalStep DROP CONSTRAINT [DF__GoalStep__Comput__44CA3770]");
            AlterColumn("dbo.GoalStep", "ComputingValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddPrimaryKey("dbo.GoodsCategoryGoods", new[] { "GoodsCategoryId", "GoodsId" });
            DropColumn("dbo.Goal", "SupplierGoalId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goal", "SupplierGoalId", c => c.Int());
            DropPrimaryKey("dbo.GoodsCategoryGoods");
            AlterColumn("dbo.GoalStep", "ComputingValue", c => c.Single(nullable: false));
            AddPrimaryKey("dbo.GoodsCategoryGoods", new[] { "Goods_Id", "GoodsCategory_Id" });
            RenameIndex(table: "dbo.GoodsCategoryGoods", name: "IX_GoodsId", newName: "IX_Goods_Id");
            RenameIndex(table: "dbo.GoodsCategoryGoods", name: "IX_GoodsCategoryId", newName: "IX_GoodsCategory_Id");
            RenameColumn(table: "dbo.GoodsCategoryGoods", name: "GoodsCategoryId", newName: "GoodsCategory_Id");
            RenameColumn(table: "dbo.GoodsCategoryGoods", name: "GoodsId", newName: "Goods_Id");
            CreateIndex("dbo.Goal", "SupplierGoalId");
            AddForeignKey("dbo.Goal", "SupplierGoalId", "dbo.Goal", "Id");
            RenameTable(name: "dbo.GoodsCategoryGoods", newName: "GoodsGoodsCategory");
        }
    }
}
