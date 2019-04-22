namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterTotalNumber_branchsales : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BranchSales", "TotalNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BranchSales", "TotalNumber", c => c.Double(nullable: false));
        }
    }
}
