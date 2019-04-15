namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ranmeTotalRange_Totalfulfill : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TotalSalesGoalRange", "GoalId", "dbo.Goal");
            DropIndex("dbo.TotalSalesGoalRange", new[] { "GoalId" });
            CreateTable(
                "dbo.TotalFulfillPromotionPercent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        TotalSalesFulfilledPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceiptFulfilledPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrivateReceiptFulfilledPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AggregateFulfilledPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchManagerPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellerPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.TotalSalesGoalRange");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TotalSalesGoalRange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        StartTotalSalesRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartReceiptRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartAggregateRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromotionTotalSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromotionTotalReceipt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromotionAggregate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.TotalFulfillPromotionPercent");
            CreateIndex("dbo.TotalSalesGoalRange", "GoalId");
            AddForeignKey("dbo.TotalSalesGoalRange", "GoalId", "dbo.Goal", "Id");
        }
    }
}
