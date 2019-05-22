namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_branchgoalId_add_branchId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BranchGoalPromotion", new[] { "BranchGoalId" });
            RenameColumn(table: "dbo.BranchGoalPromotion", name: "BranchGoalId", newName: "BranchGoal_Id");
            AddColumn("dbo.BranchGoalPromotion", "BranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.BranchGoalPromotion", "BranchGoal_Id", c => c.Int());
            CreateIndex("dbo.BranchGoalPromotion", "BranchId");
            CreateIndex("dbo.BranchGoalPromotion", "BranchGoal_Id");
            AddForeignKey("dbo.BranchGoalPromotion", "BranchId", "dbo.Branch", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchGoalPromotion", "BranchId", "dbo.Branch");
            DropIndex("dbo.BranchGoalPromotion", new[] { "BranchGoal_Id" });
            DropIndex("dbo.BranchGoalPromotion", new[] { "BranchId" });
            AlterColumn("dbo.BranchGoalPromotion", "BranchGoal_Id", c => c.Int(nullable: false));
            DropColumn("dbo.BranchGoalPromotion", "BranchId");
            RenameColumn(table: "dbo.BranchGoalPromotion", name: "BranchGoal_Id", newName: "BranchGoalId");
            CreateIndex("dbo.BranchGoalPromotion", "BranchGoalId");
        }
    }
}
