namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_quantityCond : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BranchQuantityCondition", new[] { "GoodsQuantityConditionId" });
            AlterColumn("dbo.BranchQuantityCondition", "GoodsQuantityConditionId", c => c.Int());
            CreateIndex("dbo.BranchQuantityCondition", "GoodsQuantityConditionId");
            DropColumn("dbo.GoodsQuantityCondition", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoodsQuantityCondition", "Quantity", c => c.Int(nullable: false));
            DropIndex("dbo.BranchQuantityCondition", new[] { "GoodsQuantityConditionId" });
            AlterColumn("dbo.BranchQuantityCondition", "GoodsQuantityConditionId", c => c.Int(nullable: false));
            CreateIndex("dbo.BranchQuantityCondition", "GoodsQuantityConditionId");
        }
    }
}
