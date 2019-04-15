namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAmount_branchGoal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchGoal", "Amount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.BranchGoal", "Percent", c => c.Decimal(precision: 9, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BranchGoal", "Percent", c => c.Decimal(nullable: false, precision: 9, scale: 3));
            DropColumn("dbo.BranchGoal", "Amount");
        }
    }
}
