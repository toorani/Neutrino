namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationGoalAndBranchBenefit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchBenefit", "GoalId", c => c.Int(nullable: false));
            CreateIndex("dbo.BranchBenefit", "GoalId");
            AddForeignKey("dbo.BranchBenefit", "GoalId", "dbo.Goal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchBenefit", "GoalId", "dbo.Goal");
            DropIndex("dbo.BranchBenefit", new[] { "GoalId" });
            DropColumn("dbo.BranchBenefit", "GoalId");
        }
    }
}
