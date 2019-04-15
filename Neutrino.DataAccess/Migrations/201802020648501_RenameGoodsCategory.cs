namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameGoodsCategory : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GoodsCategory", newName: "GoalGoodsCategory");
            RenameTable(name: "dbo.GoodsCategoryType", newName: "GoalGoodsCategoryType");
            RenameTable(name: "dbo.GoodsCategoryGoods", newName: "GoalGoodsCategoryGoods");
            RenameTable(name: "dbo.BranchBenefitGoodsCategory", newName: "BranchBenefitGoalGoodsCategory");
            AddColumn("dbo.GoodsFormType", "Id1", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceLog", "Exception", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceLog", "Exception", c => c.String(maxLength: 2000));
            DropColumn("dbo.GoodsFormType", "Id1");
            RenameTable(name: "dbo.BranchBenefitGoalGoodsCategory", newName: "BranchBenefitGoodsCategory");
            RenameTable(name: "dbo.GoalGoodsCategoryGoods", newName: "GoodsCategoryGoods");
            RenameTable(name: "dbo.GoalGoodsCategoryType", newName: "GoodsCategoryType");
            RenameTable(name: "dbo.GoalGoodsCategory", newName: "GoodsCategory");
        }
    }
}
