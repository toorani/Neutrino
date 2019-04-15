namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterGoal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TotalSalesCalculatedPromotion", "ReceiptTotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesCalculatedPromotion", "Touch_TotalReceiptPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesCalculatedPromotion", "Promotion_TotalReceipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesGoalRange", "PromotionTotalReceipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BranchReceipt", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.TotalSalesCalculatedPromotion", "ReceiptAmount");
            DropColumn("dbo.TotalSalesCalculatedPromotion", "Touch_ReceiptPercent");
            DropColumn("dbo.TotalSalesCalculatedPromotion", "Promotion_Receipt");
            DropColumn("dbo.TotalSalesGoalRange", "PromotionReceipt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TotalSalesGoalRange", "PromotionReceipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesCalculatedPromotion", "Promotion_Receipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesCalculatedPromotion", "Touch_ReceiptPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesCalculatedPromotion", "ReceiptAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BranchReceipt", "TotalAmount", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.TotalSalesGoalRange", "PromotionTotalReceipt");
            DropColumn("dbo.TotalSalesCalculatedPromotion", "Promotion_TotalReceipt");
            DropColumn("dbo.TotalSalesCalculatedPromotion", "Touch_TotalReceiptPercent");
            DropColumn("dbo.TotalSalesCalculatedPromotion", "ReceiptTotalAmount");
        }
    }
}
