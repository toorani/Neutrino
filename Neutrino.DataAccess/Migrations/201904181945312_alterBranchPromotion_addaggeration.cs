namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterBranchPromotion_addaggeration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchPromotion", "PrivateReceiptSpecifiedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationSalesAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationSpecifiedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationSpecifiedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationPromotionPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BranchPromotion", "PrivateReceiptAmountSpecified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BranchPromotion", "PrivateReceiptAmountSpecified", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BranchPromotion", "AggregationPromotionPercent");
            DropColumn("dbo.BranchPromotion", "AggregationSpecifiedAmount");
            DropColumn("dbo.BranchPromotion", "AggregationSpecifiedPercent");
            DropColumn("dbo.BranchPromotion", "AggregationReachedPercent");
            DropColumn("dbo.BranchPromotion", "AggregationSalesAmount");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptSpecifiedAmount");
        }
    }
}
