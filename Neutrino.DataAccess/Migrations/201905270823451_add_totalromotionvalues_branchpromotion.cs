namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_totalromotionvalues_branchpromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchPromotion", "TotalSalesPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "PrivateReceiptPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalReceiptPromotion", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchPromotion", "TotalReceiptPromotion");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptPromotion");
            DropColumn("dbo.BranchPromotion", "TotalSalesPromotion");
        }
    }
}
