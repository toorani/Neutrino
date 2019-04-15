namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_branchbenefit_branchgoal : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BranchBenefit", newName: "BranchGoal");
            AddColumn("dbo.Goal", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Goal", "Month", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Goal", "Month");
            DropColumn("dbo.Goal", "Year");
            RenameTable(name: "dbo.BranchGoal", newName: "BranchBenefit");
        }
    }
}
