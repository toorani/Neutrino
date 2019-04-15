namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterTotalfulfillment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GoalFulfillment", "TouchedGoalPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.GoalFulfillment", "EncouragePercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "TotalSalesFulfilledPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "TotalReceiptFulfilledPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "PrivateReceiptFulfilledPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "AggregateFulfilledPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "BranchManagerPromotion", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "SellerPromotion", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.TotalFulfillPromotionPercent", "Month");
            DropColumn("dbo.TotalFulfillPromotionPercent", "Year");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TotalFulfillPromotionPercent", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.TotalFulfillPromotionPercent", "Month", c => c.Int(nullable: false));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "SellerPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "BranchManagerPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "AggregateFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "PrivateReceiptFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "TotalReceiptFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.TotalFulfillPromotionPercent", "TotalSalesFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GoalFulfillment", "EncouragePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GoalFulfillment", "TouchedGoalPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
