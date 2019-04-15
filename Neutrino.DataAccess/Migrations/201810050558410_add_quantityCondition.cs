namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_quantityCondition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchQuantityCondition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        GoodsQuantityConditionId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.GoodsQuantityCondition", t => t.GoodsQuantityConditionId)
                .Index(t => t.BranchId)
                .Index(t => t.GoodsQuantityConditionId);
            
            CreateTable(
                "dbo.GoodsQuantityCondition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        QuantityCondition_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .ForeignKey("dbo.QuantityCondition", t => t.QuantityCondition_Id)
                .Index(t => t.GoodsId)
                .Index(t => t.QuantityCondition_Id);
            
            CreateTable(
                "dbo.QuantityCondition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ExtraEncouragePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.GoalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoodsQuantityCondition", "QuantityCondition_Id", "dbo.QuantityCondition");
            DropForeignKey("dbo.QuantityCondition", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.GoodsQuantityCondition", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.BranchQuantityCondition", "GoodsQuantityConditionId", "dbo.GoodsQuantityCondition");
            DropForeignKey("dbo.BranchQuantityCondition", "BranchId", "dbo.Branch");
            DropIndex("dbo.QuantityCondition", new[] { "GoalId" });
            DropIndex("dbo.GoodsQuantityCondition", new[] { "QuantityCondition_Id" });
            DropIndex("dbo.GoodsQuantityCondition", new[] { "GoodsId" });
            DropIndex("dbo.BranchQuantityCondition", new[] { "GoodsQuantityConditionId" });
            DropIndex("dbo.BranchQuantityCondition", new[] { "BranchId" });
            DropTable("dbo.QuantityCondition");
            DropTable("dbo.GoodsQuantityCondition");
            DropTable("dbo.BranchQuantityCondition");
        }
    }
}
