namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterGoalNonFulfillment : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GoalNonFulfillmentBranch", new[] { "GoalNonFulfillmentPercent_Id" });
            RenameColumn(table: "dbo.GoalNonFulfillmentBranch", name: "GoalNonFulfillmentPercent_Id", newName: "GoalNonFulfillmentPercentId");
            AlterColumn("dbo.GoalNonFulfillmentBranch", "GoalNonFulfillmentPercentId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoalNonFulfillmentBranch", "GoalNonFulfillmentPercentId");
            DropColumn("dbo.GoalNonFulfillmentBranch", "GoalNonFullfilmentPercentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalNonFulfillmentBranch", "GoalNonFullfilmentPercentId", c => c.Int(nullable: false));
            DropIndex("dbo.GoalNonFulfillmentBranch", new[] { "GoalNonFulfillmentPercentId" });
            AlterColumn("dbo.GoalNonFulfillmentBranch", "GoalNonFulfillmentPercentId", c => c.Int());
            RenameColumn(table: "dbo.GoalNonFulfillmentBranch", name: "GoalNonFulfillmentPercentId", newName: "GoalNonFulfillmentPercent_Id");
            CreateIndex("dbo.GoalNonFulfillmentBranch", "GoalNonFulfillmentPercent_Id");
        }
    }
}
