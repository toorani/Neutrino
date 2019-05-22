namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFulfillPercent_branchgoalpromotion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BranchGoalPromotion", "BranchGoal_Id", "dbo.BranchGoal");
            DropForeignKey("dbo.MemberSales", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.MemberSales", "MemberId", "dbo.Member");
            DropIndex("dbo.BranchGoalPromotion", new[] { "BranchGoal_Id" });
            DropIndex("dbo.MemberSales", new[] { "MemberId" });
            DropIndex("dbo.MemberSales", new[] { "GoodsId" });
            AddColumn("dbo.BranchGoalPromotion", "FulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BranchGoalPromotion", "BranchGoal_Id");
            //DropTable("dbo.MemberSales");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BranchGoalPromotion", "BranchGoal_Id", c => c.Int());
            DropColumn("dbo.BranchGoalPromotion", "FulfilledPercent");
            CreateIndex("dbo.MemberSales", "GoodsId");
            CreateIndex("dbo.MemberSales", "MemberId");
            CreateIndex("dbo.BranchGoalPromotion", "BranchGoal_Id");
            AddForeignKey("dbo.MemberSales", "MemberId", "dbo.Member", "Id");
            AddForeignKey("dbo.MemberSales", "GoodsId", "dbo.Goods", "Id");
            AddForeignKey("dbo.BranchGoalPromotion", "BranchGoal_Id", "dbo.BranchGoal", "Id");
        }
    }
}
