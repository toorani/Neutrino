namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRelationBranchBenefit_GoalGoodsCat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BranchBenefitGoalGoodsCategory", "BranchBenefitId", "dbo.BranchBenefit");
            DropForeignKey("dbo.BranchBenefitGoalGoodsCategory", "GoodsCategoryId", "dbo.GoalGoodsCategory");
            DropIndex("dbo.BranchBenefitGoalGoodsCategory", new[] { "BranchBenefitId" });
            DropIndex("dbo.BranchBenefitGoalGoodsCategory", new[] { "GoodsCategoryId" });
            AddColumn("dbo.BranchBenefit", "GoalGoodsCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.BranchBenefit", "GoalGoodsCategoryId");
            AddForeignKey("dbo.BranchBenefit", "GoalGoodsCategoryId", "dbo.GoalGoodsCategory", "Id");
            DropTable("dbo.BranchBenefitGoalGoodsCategory");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BranchBenefitGoalGoodsCategory",
                c => new
                    {
                        BranchBenefitId = c.Int(nullable: false),
                        GoodsCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchBenefitId, t.GoodsCategoryId });
            
            DropForeignKey("dbo.BranchBenefit", "GoalGoodsCategoryId", "dbo.GoalGoodsCategory");
            DropIndex("dbo.BranchBenefit", new[] { "GoalGoodsCategoryId" });
            DropColumn("dbo.BranchBenefit", "GoalGoodsCategoryId");
            CreateIndex("dbo.BranchBenefitGoalGoodsCategory", "GoodsCategoryId");
            CreateIndex("dbo.BranchBenefitGoalGoodsCategory", "BranchBenefitId");
            AddForeignKey("dbo.BranchBenefitGoalGoodsCategory", "GoodsCategoryId", "dbo.GoalGoodsCategory", "Id");
            AddForeignKey("dbo.BranchBenefitGoalGoodsCategory", "BranchBenefitId", "dbo.BranchBenefit", "Id");
        }
    }
}
