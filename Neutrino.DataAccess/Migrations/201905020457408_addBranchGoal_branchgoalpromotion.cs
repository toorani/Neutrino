namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBranchGoal_branchgoalpromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchGoalPromotion", "BranchGoalId", c => c.Int(nullable: false));
            CreateIndex("dbo.BranchGoalPromotion", "BranchGoalId");
            AddForeignKey("dbo.BranchGoalPromotion", "BranchGoalId", "dbo.BranchGoal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchGoalPromotion", "BranchGoalId", "dbo.BranchGoal");
            DropIndex("dbo.BranchGoalPromotion", new[] { "BranchGoalId" });
            DropColumn("dbo.BranchGoalPromotion", "BranchGoalId");
        }
    }
}
