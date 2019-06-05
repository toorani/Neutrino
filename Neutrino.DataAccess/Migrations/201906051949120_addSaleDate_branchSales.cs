namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSaleDate_branchSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchSales", "SalesDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchSales", "SalesDate");
        }
    }
}
