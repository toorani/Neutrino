namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMemberSales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchPromotionId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Promotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchPromotion", t => t.BranchPromotionId)
                .Index(t => t.BranchPromotionId);
            
            CreateTable(
                "dbo.MemberSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        MemberRefId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        GoodsRefId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId)
                .Index(t => t.GoodsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberSales", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberSales", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.MemberPromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropIndex("dbo.MemberSales", new[] { "GoodsId" });
            DropIndex("dbo.MemberSales", new[] { "MemberId" });
            DropIndex("dbo.MemberPromotion", new[] { "BranchPromotionId" });
            DropTable("dbo.MemberSales");
            DropTable("dbo.MemberPromotion");
        }
    }
}
