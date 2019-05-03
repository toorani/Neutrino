namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRelationSomeEntities : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.BranchGoalPromotion", "GoalId");
            AddForeignKey("dbo.BranchGoalPromotion", "GoalId", "dbo.Goal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchGoalPromotion", "GoalId", "dbo.Goal");
            DropIndex("dbo.BranchGoalPromotion", new[] { "GoalId" });
        }
    }
}
