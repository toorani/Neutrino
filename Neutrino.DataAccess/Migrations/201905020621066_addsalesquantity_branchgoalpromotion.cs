namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsalesquantity_branchgoalpromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchPromotion", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BranchPromotion", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BranchGoalPromotion", "TotalSales", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchGoalPromotion", "TotalQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchGoalPromotion", "TotalQuantity");
            DropColumn("dbo.BranchGoalPromotion", "TotalSales");
            DropColumn("dbo.BranchPromotion", "EndDate");
            DropColumn("dbo.BranchPromotion", "StartDate");
        }
    }
}
