namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class add_membersharepromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberSharePromotion",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BranchPromotionId = c.Int(nullable: false),
                    MemberId = c.Int(nullable: false),
                    ManagerPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CEOPromotion = c.Decimal(precision: 18, scale: 2),
                    FinalPromotion = c.Decimal(precision: 18, scale: 2),
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
                "dbo.PromotionReviewStatus",
                c => new
                {
                    Id = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    Description = c.String(maxLength: 100),
                    Code = c.Int(),
                    DateCreated = c.DateTime(),
                    CreatorID = c.Int(),
                    Deleted = c.Boolean(nullable: false),
                    LastUpdated = c.DateTime(),
                    EditorId = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.BranchPromotion", "PromotionReviewStatusId", c => c.Int(nullable: true, defaultValue: 1));
            CreateIndex("dbo.BranchPromotion", "PromotionReviewStatusId");
            AddForeignKey("dbo.BranchPromotion", "PromotionReviewStatusId", "dbo.PromotionReviewStatus", "Id");
            DropColumn("dbo.BranchPromotion", "TotalReceiptSpecifiedAmount");
            DropColumn("dbo.BranchPromotion", "TotalReceiptReachedPercent");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptSpecifiedAmount");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptAmount");
            DropColumn("dbo.BranchPromotion", "PrivateReceiptReachedPercent");
            DropColumn("dbo.BranchPromotion", "TotalSalesReachedPercent");
            DropColumn("dbo.BranchPromotion", "TotalSalesSpecifiedPercent");
            DropColumn("dbo.BranchPromotion", "TotalSalesSpecifiedAmount");
            DropColumn("dbo.BranchPromotion", "AggregationSalesAmount");
            DropColumn("dbo.BranchPromotion", "AggregationReachedPercent");
            DropColumn("dbo.BranchPromotion", "AggregationSpecifiedAmount");
        }

        public override void Down()
        {
            AddColumn("dbo.BranchPromotion", "AggregationSpecifiedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "AggregationSalesAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalSalesSpecifiedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalSalesSpecifiedPercent", c => c.Decimal(nullable: false, precision: 9, scale: 5));
            AddColumn("dbo.BranchPromotion", "TotalSalesReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "PrivateReceiptReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "PrivateReceiptAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "PrivateReceiptSpecifiedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalReceiptReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BranchPromotion", "TotalReceiptSpecifiedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.BranchPromotion", "PromotionReviewStatusId", "dbo.PromotionReviewStatus");
            DropForeignKey("dbo.MemberSharePromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropIndex("dbo.MemberSharePromotion", new[] { "BranchPromotionId" });
            DropIndex("dbo.BranchPromotion", new[] { "PromotionReviewStatusId" });
            DropColumn("dbo.BranchPromotion", "PromotionReviewStatusId");
            DropTable("dbo.PromotionReviewStatus");
            DropTable("dbo.MemberSharePromotion");
        }
    }
}
