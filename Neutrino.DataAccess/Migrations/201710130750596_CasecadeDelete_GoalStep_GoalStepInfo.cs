namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CasecadeDelete_GoalStep_GoalStepInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep");
            AddForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep");
            AddForeignKey("dbo.GoalStepItemInfo", "GoalStepId", "dbo.GoalStep", "Id");
        }
    }
}
