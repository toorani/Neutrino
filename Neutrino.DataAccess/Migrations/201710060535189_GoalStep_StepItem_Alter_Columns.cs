namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GoalStep_StepItem_Alter_Columns : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GoalStep", new[] { "RewardInfoId" });
            DropIndex("dbo.GoalStepItemInfo", new[] { "ComputingTypeId" });
            AddColumn("dbo.GoalStepItemInfo", "GoalStepId", c => c.Int(nullable: false));
            AlterColumn("dbo.GoalStep", "RewardInfoId", c => c.Int());
            AlterColumn("dbo.GoalStepItemInfo", "ComputingTypeId", c => c.Int());
            AlterColumn("dbo.GoalStepItemInfo", "EachValue", c => c.Int());
            CreateIndex("dbo.GoalStep", "RewardInfoId");
            CreateIndex("dbo.GoalStepItemInfo", "ComputingTypeId");
            CreateIndex("dbo.GoalStepItemInfo", "GoalStepId");
            AddForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep");
            DropIndex("dbo.GoalStepItemInfo", new[] { "GoalStepId" });
            DropIndex("dbo.GoalStepItemInfo", new[] { "ComputingTypeId" });
            DropIndex("dbo.GoalStep", new[] { "RewardInfoId" });
            AlterColumn("dbo.GoalStepItemInfo", "EachValue", c => c.Int(nullable: false));
            AlterColumn("dbo.GoalStepItemInfo", "ComputingTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.GoalStep", "RewardInfoId", c => c.Int(nullable: false));
            DropColumn("dbo.GoalStepItemInfo", "GoalStepId");
            CreateIndex("dbo.GoalStepItemInfo", "ComputingTypeId");
            CreateIndex("dbo.GoalStep", "RewardInfoId");
        }
    }
}
