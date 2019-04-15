namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterGoalFullfilment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoalGoalFulfillment", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.GoalGoalFulfillment", "GoalFulfillmentId", "dbo.GoalFulfillment");
            DropIndex("dbo.GoalGoalFulfillment", new[] { "GoalFulfillmentId" });
            DropIndex("dbo.GoalGoalFulfillment", new[] { "GoalId" });
            AddColumn("dbo.GoalFulfillment", "GoalId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoalFulfillment", "GoalId");
            AddForeignKey("dbo.GoalFulfillment", "GoalId", "dbo.Goal", "Id");
            DropTable("dbo.GoalGoalFulfillment");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GoalGoalFulfillment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalFulfillmentId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.GoalFulfillment", "GoalId", "dbo.Goal");
            DropIndex("dbo.GoalFulfillment", new[] { "GoalId" });
            DropColumn("dbo.GoalFulfillment", "GoalId");
            CreateIndex("dbo.GoalGoalFulfillment", "GoalId");
            CreateIndex("dbo.GoalGoalFulfillment", "GoalFulfillmentId");
            AddForeignKey("dbo.GoalGoalFulfillment", "GoalFulfillmentId", "dbo.GoalFulfillment", "Id");
            AddForeignKey("dbo.GoalGoalFulfillment", "GoalId", "dbo.Goal", "Id");
        }
    }
}
