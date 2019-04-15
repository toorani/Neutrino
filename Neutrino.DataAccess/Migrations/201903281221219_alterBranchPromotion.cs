namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterBranchPromotion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TotalSalesCalculatedPromotion", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.TotalSalesCalculatedPromotion", new[] { "PromotionId" });
            CreateTable(
                "dbo.BranchPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PromotionId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        TotalReceiptSpecifiedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceiptReachedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceiptPromotionPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceiptPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrivateReceiptAmountSpecified = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrivateReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrivateReceiptReachedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrivateReceiptPromotionPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrivateReceiptPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSalesAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSalesReachedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSalesSpecifiedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSalesPromotionPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSalesPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Promotion", t => t.PromotionId)
                .Index(t => t.PromotionId);
            
            DropTable("dbo.TotalSalesCalculatedPromotion");
        }
        
        public override void Down()
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
                        ReceiptTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesTotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Goal_TotalSalesPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Goal_ReceiptPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Touch_TotalSalesPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Touch_TotalReceiptPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Promotion_TotalSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Promotion_TotalReceipt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.BranchPromotion", "PromotionId", "dbo.Promotion");
            DropIndex("dbo.BranchPromotion", new[] { "PromotionId" });
            DropTable("dbo.BranchPromotion");
            CreateIndex("dbo.TotalSalesCalculatedPromotion", "PromotionId");
            AddForeignKey("dbo.TotalSalesCalculatedPromotion", "PromotionId", "dbo.Promotion", "Id");
        }
    }
}
