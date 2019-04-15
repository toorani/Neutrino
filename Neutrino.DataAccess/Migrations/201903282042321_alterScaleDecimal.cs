namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterScaleDecimal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoodsScore", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.GoodsScore", "GoodsId", "dbo.Goods");
            DropIndex("dbo.GoodsScore", new[] { "CompanyId" });
            DropIndex("dbo.GoodsScore", new[] { "GoodsId" });
            AlterColumn("dbo.BranchGoal", "Percent", c => c.Decimal(precision: 9, scale: 5));
            AlterColumn("dbo.BranchReceiptGoalPercent", "ReachedPercent", c => c.Decimal(nullable: false, precision: 9, scale: 5));
            AlterColumn("dbo.BranchReceiptGoalPercent", "NotReachedPercent", c => c.Decimal(nullable: false, precision: 9, scale: 5));
            DropTable("dbo.GoodsScore");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GoodsScore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartedStock = c.Int(nullable: false),
                        AddedStock = c.Int(nullable: false),
                        Sales = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        AddedStockSalesPercent = c.Decimal(nullable: false, precision: 6, scale: 3),
                        StartedStockSalesPercent = c.Decimal(nullable: false, precision: 6, scale: 3),
                        Score = c.Decimal(nullable: false, precision: 6, scale: 3),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.BranchReceiptGoalPercent", "NotReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BranchReceiptGoalPercent", "ReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BranchGoal", "Percent", c => c.Decimal(precision: 9, scale: 3));
            CreateIndex("dbo.GoodsScore", "GoodsId");
            CreateIndex("dbo.GoodsScore", "CompanyId");
            AddForeignKey("dbo.GoodsScore", "GoodsId", "dbo.Goods", "Id");
            AddForeignKey("dbo.GoodsScore", "CompanyId", "dbo.Company", "Id");
        }
    }
}
