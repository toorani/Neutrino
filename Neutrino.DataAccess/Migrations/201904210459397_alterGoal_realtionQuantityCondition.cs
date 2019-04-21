namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterGoal_realtionQuantityCondition : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BranchPromotion", "TotalReceiptAmount");
            DropColumn("dbo.BranchPromotion", "TotalReceiptPromotionPercent");
            DropColumn("dbo.BranchPromotion", "TotalReceiptPromotion");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptPromotionPercent");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptPromotion");
            DropColumn("dbo.BranchPromotion", "TotalSalesAmount");
            DropColumn("dbo.BranchPromotion", "TotalSalesPromotionPercent");
            DropColumn("dbo.BranchPromotion", "AggregationSpecifiedPercent");
            DropColumn("dbo.BranchPromotion", "AggregationPromotionPercent");
            DropColumn("dbo.BranchPromotion", "TotalSalesPromotion");
            DropColumn("dbo.BranchPromotion", "SalesPromotion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BranchPromotion", "SalesPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalSalesPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationPromotionPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationSpecifiedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalSalesPromotionPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalSalesAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "PrivateReceiptPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "PrivateReceiptPromotionPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalReceiptPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalReceiptPromotionPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalReceiptAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
