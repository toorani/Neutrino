namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_suppplierpromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchPromotion", "SupplierPromotion", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.BranchPromotion", "TotalSalesPromotion", c => c.Decimal(nullable: true, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BranchPromotion", "TotalSalesPromotion", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.BranchPromotion", "SupplierPromotion");
        }
    }
}
