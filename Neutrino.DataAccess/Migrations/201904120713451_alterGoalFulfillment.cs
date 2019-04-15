namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterGoalFulfillment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TotalFulfillPromotionPercent", newName: "FulfillmentPromotionCondition");
            RenameTable(name: "dbo.GoalFulfillment", newName: "FulfillmentPercent");
            DropForeignKey("dbo.GoalFulfillment", "GoalId", "dbo.Goal");
            DropIndex("dbo.FulfillmentPercent", new[] { "GoalId" });
            AddColumn("dbo.FulfillmentPromotionCondition", "ManagerPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "ManagerReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "SellerReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "ManagerFulfillmentPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "SellerFulfillmentPercent", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.FulfillmentPromotionCondition", "BranchManagerPromotion");
            DropColumn("dbo.FulfillmentPercent", "TouchedGoalPercent");
            DropColumn("dbo.FulfillmentPercent", "EncouragePercent");
            DropColumn("dbo.FulfillmentPercent", "GoalId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FulfillmentPercent", "GoalId", c => c.Int(nullable: false));
            AddColumn("dbo.FulfillmentPercent", "EncouragePercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "TouchedGoalPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPromotionCondition", "BranchManagerPromotion", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.FulfillmentPercent", "SellerFulfillmentPercent");
            DropColumn("dbo.FulfillmentPercent", "ManagerFulfillmentPercent");
            DropColumn("dbo.FulfillmentPercent", "SellerReachedPercent");
            DropColumn("dbo.FulfillmentPercent", "ManagerReachedPercent");
            DropColumn("dbo.FulfillmentPromotionCondition", "ManagerPromotion");
            CreateIndex("dbo.FulfillmentPercent", "GoalId");
            AddForeignKey("dbo.GoalFulfillment", "GoalId", "dbo.Goal", "Id");
            RenameTable(name: "dbo.FulfillmentPercent", newName: "GoalFulfillment");
            RenameTable(name: "dbo.FulfillmentPromotionCondition", newName: "TotalFulfillPromotionPercent");
        }
    }
}
