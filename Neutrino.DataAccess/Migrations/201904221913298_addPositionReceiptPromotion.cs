namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPositionReceiptPromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PositionReceiptPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchGoalPromotionId = c.Int(nullable: false),
                        OrgStructureShareId = c.Int(nullable: false),
                        Promotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchGoalPromotion", t => t.BranchGoalPromotionId)
                .ForeignKey("dbo.OrgStructureShare", t => t.OrgStructureShareId)
                .Index(t => t.BranchGoalPromotionId)
                .Index(t => t.OrgStructureShareId);
            
            AlterColumn("dbo.OrgStructureShare", "SalesPercent", c => c.Decimal(precision: 9, scale: 5));
            AlterColumn("dbo.OrgStructureShare", "PrivateReceiptPercent", c => c.Decimal(precision: 9, scale: 5));
            AlterColumn("dbo.OrgStructureShare", "TotalReceiptPercent", c => c.Decimal(precision: 9, scale: 5));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PositionReceiptPromotion", "OrgStructureShareId", "dbo.OrgStructureShare");
            DropForeignKey("dbo.PositionReceiptPromotion", "BranchGoalPromotionId", "dbo.BranchGoalPromotion");
            DropIndex("dbo.PositionReceiptPromotion", new[] { "OrgStructureShareId" });
            DropIndex("dbo.PositionReceiptPromotion", new[] { "BranchGoalPromotionId" });
            AlterColumn("dbo.OrgStructureShare", "TotalReceiptPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.OrgStructureShare", "PrivateReceiptPercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.OrgStructureShare", "SalesPercent", c => c.Decimal(precision: 18, scale: 2));
            DropTable("dbo.PositionReceiptPromotion");
        }
    }
}
