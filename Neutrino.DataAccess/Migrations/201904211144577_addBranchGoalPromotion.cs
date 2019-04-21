namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBranchGoalPromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchGoalPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchPromotionId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                        PromotionWithOutFulfillmentPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinalPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchPromotion", t => t.BranchPromotionId)
                .Index(t => t.BranchPromotionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchGoalPromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropIndex("dbo.BranchGoalPromotion", new[] { "BranchPromotionId" });
            DropTable("dbo.BranchGoalPromotion");
        }
    }
}
