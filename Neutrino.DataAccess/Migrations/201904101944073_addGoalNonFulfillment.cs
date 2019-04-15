namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGoalNonFulfillment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoalNonFulfillmentPercent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.GoalId);
            
            CreateTable(
                "dbo.GoalNonFulfillmentBranch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalNonFullfilmentPercentId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                        GoalNonFulfillmentPercent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.GoalNonFulfillmentPercent", t => t.GoalNonFulfillmentPercent_Id)
                .Index(t => t.BranchId)
                .Index(t => t.GoalNonFulfillmentPercent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalNonFulfillmentBranch", "GoalNonFulfillmentPercent_Id", "dbo.GoalNonFulfillmentPercent");
            DropForeignKey("dbo.GoalNonFulfillmentBranch", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.GoalNonFulfillmentPercent", "GoalId", "dbo.Goal");
            DropIndex("dbo.GoalNonFulfillmentBranch", new[] { "GoalNonFulfillmentPercent_Id" });
            DropIndex("dbo.GoalNonFulfillmentBranch", new[] { "BranchId" });
            DropIndex("dbo.GoalNonFulfillmentPercent", new[] { "GoalId" });
            DropTable("dbo.GoalNonFulfillmentBranch");
            DropTable("dbo.GoalNonFulfillmentPercent");
        }
    }
}
