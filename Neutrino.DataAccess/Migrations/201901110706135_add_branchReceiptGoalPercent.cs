namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_branchReceiptGoalPercent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoodsPromotion", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.GoodsPromotion", "GoodsPriceId", "dbo.GoodsPrice");
            DropForeignKey("dbo.StockInventory", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.StockInventory", "GoodsId", "dbo.Goods");
            DropIndex("dbo.GoodsPromotion", new[] { "GoodsId" });
            DropIndex("dbo.GoodsPromotion", new[] { "GoodsPriceId" });
            DropIndex("dbo.StockInventory", new[] { "GoodsId" });
            DropIndex("dbo.StockInventory", new[] { "CompanyId" });
            CreateTable(
                "dbo.BranchReceiptGoalPercent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        ReachedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NotReachedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GoalId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.BranchId)
                .Index(t => t.GoalId);
            
            DropTable("dbo.GoodsPromotion");
            DropTable("dbo.StockInventory");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StockInventory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoodsPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        GoodsPriceId = c.Int(nullable: false),
                        PromotionValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Eran = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.BranchReceiptGoalPercent", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.BranchReceiptGoalPercent", "BranchId", "dbo.Branch");
            DropIndex("dbo.BranchReceiptGoalPercent", new[] { "GoalId" });
            DropIndex("dbo.BranchReceiptGoalPercent", new[] { "BranchId" });
            DropTable("dbo.BranchReceiptGoalPercent");
            CreateIndex("dbo.StockInventory", "CompanyId");
            CreateIndex("dbo.StockInventory", "GoodsId");
            CreateIndex("dbo.GoodsPromotion", "GoodsPriceId");
            CreateIndex("dbo.GoodsPromotion", "GoodsId");
            AddForeignKey("dbo.StockInventory", "GoodsId", "dbo.Goods", "Id");
            AddForeignKey("dbo.StockInventory", "CompanyId", "dbo.Company", "Id");
            AddForeignKey("dbo.GoodsPromotion", "GoodsPriceId", "dbo.GoodsPrice", "Id");
            AddForeignKey("dbo.GoodsPromotion", "GoodsId", "dbo.Goods", "Id");
        }
    }
}
