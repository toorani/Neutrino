namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderToBrnach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branch", "Order", c => c.String());
            AlterColumn("dbo.BranchPromotion", "TotalSalesSpecifiedPercent", c => c.Decimal(nullable: false, precision: 9, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BranchPromotion", "TotalSalesSpecifiedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Branch", "Order");
        }
    }
}
