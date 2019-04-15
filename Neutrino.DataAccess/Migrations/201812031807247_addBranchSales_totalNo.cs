namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBranchSales_totalNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchSales", "TotalNumber", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchSales", "TotalNumber");
        }
    }
}
