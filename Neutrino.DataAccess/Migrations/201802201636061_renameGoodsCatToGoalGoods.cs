namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameGoodsCatToGoalGoods : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Goal", name: "GoodsCategoryId", newName: "GoalGoodsCategoryId");
            RenameColumn(table: "dbo.Goal", name: "GoodsCategoryTypeId", newName: "GoalGoodsCategoryTypeId");
            RenameColumn(table: "dbo.GoalGoodsCategory", name: "GoodsCategoryTypeId", newName: "GoalGoodsCategoryTypeId");
            RenameIndex(table: "dbo.GoalGoodsCategory", name: "IX_GoodsCategoryTypeId", newName: "IX_GoalGoodsCategoryTypeId");
            RenameIndex(table: "dbo.Goal", name: "IX_GoodsCategoryId", newName: "IX_GoalGoodsCategoryId");
            RenameIndex(table: "dbo.Goal", name: "IX_GoodsCategoryTypeId", newName: "IX_GoalGoodsCategoryTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Goal", name: "IX_GoalGoodsCategoryTypeId", newName: "IX_GoodsCategoryTypeId");
            RenameIndex(table: "dbo.Goal", name: "IX_GoalGoodsCategoryId", newName: "IX_GoodsCategoryId");
            RenameIndex(table: "dbo.GoalGoodsCategory", name: "IX_GoalGoodsCategoryTypeId", newName: "IX_GoodsCategoryTypeId");
            RenameColumn(table: "dbo.GoalGoodsCategory", name: "GoalGoodsCategoryTypeId", newName: "GoodsCategoryTypeId");
            RenameColumn(table: "dbo.Goal", name: "GoalGoodsCategoryTypeId", newName: "GoodsCategoryTypeId");
            RenameColumn(table: "dbo.Goal", name: "GoalGoodsCategoryId", newName: "GoodsCategoryId");
        }
    }
}
