namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerGoal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        BranchRefId = c.Int(nullable: false),
                        SellerId = c.Int(nullable: false),
                        SellerRefId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        GoodsRefId = c.Int(nullable: false),
                        CustomerCount = c.Int(nullable: false),
                        ReachedCount = c.Int(nullable: false),
                        ReachedPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesCount = c.Int(nullable: false),
                        Promotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .ForeignKey("dbo.Member", t => t.SellerId)
                .Index(t => t.BranchId)
                .Index(t => t.SellerId)
                .Index(t => t.GoodsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerGoal", "SellerId", "dbo.Member");
            DropForeignKey("dbo.CustomerGoal", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.CustomerGoal", "BranchId", "dbo.Branch");
            DropIndex("dbo.CustomerGoal", new[] { "GoodsId" });
            DropIndex("dbo.CustomerGoal", new[] { "SellerId" });
            DropIndex("dbo.CustomerGoal", new[] { "BranchId" });
            DropTable("dbo.CustomerGoal");
        }
    }
}
