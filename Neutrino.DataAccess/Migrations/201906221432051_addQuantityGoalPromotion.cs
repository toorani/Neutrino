namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQuantityGoalPromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuantityGoalPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchPromotionId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        TotalSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalQuantity = c.Int(nullable: false),
                        FulfilledPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GoodsId = c.Int(nullable: false),
                        GoalQuantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.BranchPromotion", t => t.BranchPromotionId)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.BranchPromotionId)
                .Index(t => t.GoalId)
                .Index(t => t.BranchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuantityGoalPromotion", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.QuantityGoalPromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropForeignKey("dbo.QuantityGoalPromotion", "BranchId", "dbo.Branch");
            DropIndex("dbo.QuantityGoalPromotion", new[] { "BranchId" });
            DropIndex("dbo.QuantityGoalPromotion", new[] { "GoalId" });
            DropIndex("dbo.QuantityGoalPromotion", new[] { "BranchPromotionId" });
            DropTable("dbo.QuantityGoalPromotion");
        }
    }
}
