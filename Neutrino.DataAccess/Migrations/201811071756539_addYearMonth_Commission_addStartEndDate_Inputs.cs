namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addYearMonth_Commission_addStartEndDate_Inputs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchReceipt", "StartDate", c => c.DateTime());
            AddColumn("dbo.BranchReceipt", "EndDate", c => c.DateTime());
            AddColumn("dbo.BranchSales", "StartDate", c => c.DateTime());
            AddColumn("dbo.BranchSales", "EndDate", c => c.DateTime());
            AddColumn("dbo.Commission", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.Commission", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Invoice", "StartDate", c => c.DateTime());
            AddColumn("dbo.Invoice", "EndDate", c => c.DateTime());
            AddColumn("dbo.MemberPayroll", "StartDate", c => c.DateTime());
            AddColumn("dbo.MemberPayroll", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberPayroll", "EndDate");
            DropColumn("dbo.MemberPayroll", "StartDate");
            DropColumn("dbo.Invoice", "EndDate");
            DropColumn("dbo.Invoice", "StartDate");
            DropColumn("dbo.Commission", "Year");
            DropColumn("dbo.Commission", "Month");
            DropColumn("dbo.BranchSales", "EndDate");
            DropColumn("dbo.BranchSales", "StartDate");
            DropColumn("dbo.BranchReceipt", "EndDate");
            DropColumn("dbo.BranchReceipt", "StartDate");
        }
    }
}
