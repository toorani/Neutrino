namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_commision_Promotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TotalSalesCalculatedPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        ReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Goal_TotalSalesPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Goal_ReceiptPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Touch_TotalSalesPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Touch_ReceiptPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Promotion_TotalSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Promotion_Receipt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionId)
                .Index(t => t.PromotionId);
            
            AddColumn("dbo.TotalSalesGoalRange", "PromotionTotalSales", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesGoalRange", "PromotionReceipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchQuntityGoal", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.BranchQuntityGoal", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.TotalSalesGoalRange", "CommissionTotalSales");
            DropColumn("dbo.TotalSalesGoalRange", "CommissionReceipt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TotalSalesGoalRange", "CommissionReceipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TotalSalesGoalRange", "CommissionTotalSales", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.TotalSalesCalculatedPromotion", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.TotalSalesCalculatedPromotion", new[] { "PromotionId" });
            DropColumn("dbo.BranchQuntityGoal", "Year");
            DropColumn("dbo.BranchQuntityGoal", "Month");
            DropColumn("dbo.TotalSalesGoalRange", "PromotionReceipt");
            DropColumn("dbo.TotalSalesGoalRange", "PromotionTotalSales");
            DropTable("dbo.TotalSalesCalculatedPromotion");
        }
    }
}
