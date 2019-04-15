namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterBrnachBenefit_removeGoalGoodsCat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BranchBenefit", "GoalGoodsCategoryId", "dbo.GoalGoodsCategory");
            DropIndex("dbo.BranchBenefit", new[] { "GoalGoodsCategoryId" });
            DropColumn("dbo.BranchBenefit", "GoalGoodsCategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BranchBenefit", "GoalGoodsCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.BranchBenefit", "GoalGoodsCategoryId");
            AddForeignKey("dbo.BranchBenefit", "GoalGoodsCategoryId", "dbo.GoalGoodsCategory", "Id");
        }
    }
}
