namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRelation_GoalStep_StepItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep");
            DropIndex("dbo.GoalStepItemInfo", new[] { "GoalStepId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.GoalStepItemInfo", "GoalStepId");
            AddForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep", "Id");
        }
    }
}
