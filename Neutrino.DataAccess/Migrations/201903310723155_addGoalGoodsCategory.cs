namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGoalGoodsCategory : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GoalGoodsCategoryGoods", name: "GoodsCategoryId", newName: "GoalGoodsCategoryId");
            RenameIndex(table: "dbo.GoalGoodsCategoryGoods", name: "IX_GoodsCategoryId", newName: "IX_GoalGoodsCategoryId");
            DropPrimaryKey("dbo.GoalGoodsCategoryGoods");
            AddColumn("dbo.GoalGoodsCategoryGoods", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.GoalGoodsCategoryGoods", "DateCreated", c => c.DateTime());
            AddColumn("dbo.GoalGoodsCategoryGoods", "CreatorID", c => c.Int());
            AddColumn("dbo.GoalGoodsCategoryGoods", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoalGoodsCategoryGoods", "LastUpdated", c => c.DateTime());
            AddColumn("dbo.GoalGoodsCategoryGoods", "EditorId", c => c.Int());
            AddPrimaryKey("dbo.GoalGoodsCategoryGoods", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.GoalGoodsCategoryGoods");
            DropColumn("dbo.GoalGoodsCategoryGoods", "EditorId");
            DropColumn("dbo.GoalGoodsCategoryGoods", "LastUpdated");
            DropColumn("dbo.GoalGoodsCategoryGoods", "Deleted");
            DropColumn("dbo.GoalGoodsCategoryGoods", "CreatorID");
            DropColumn("dbo.GoalGoodsCategoryGoods", "DateCreated");
            DropColumn("dbo.GoalGoodsCategoryGoods", "Id");
            AddPrimaryKey("dbo.GoalGoodsCategoryGoods", new[] { "GoodsCategoryId", "GoodsId" });
            RenameIndex(table: "dbo.GoalGoodsCategoryGoods", name: "IX_GoalGoodsCategoryId", newName: "IX_GoodsCategoryId");
            RenameColumn(table: "dbo.GoalGoodsCategoryGoods", name: "GoalGoodsCategoryId", newName: "GoodsCategoryId");
        }
    }
}
