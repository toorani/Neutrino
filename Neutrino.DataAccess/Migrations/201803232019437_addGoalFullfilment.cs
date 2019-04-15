namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGoalFullfilment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoalPercent", "GoalId", "dbo.Goal");
            DropIndex("dbo.GoalPercent", new[] { "GoalId" });
            CreateTable(
                "dbo.GoalFulfillment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TouchedGoalPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EncouragePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        BranchId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .Index(t => t.BranchId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .ForeignKey("dbo.GoalFulfillment", t => t.GoalFulfillmentId)
                .Index(t => t.GoalFulfillmentId)
                .Index(t => t.GoalId);
            
            DropTable("dbo.GoalPercent");
            DropTable("dbo.ServiceLog");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Exception = c.String(maxLength: 4000),
                        ServiceName = c.String(maxLength: 50),
                        StatusId = c.Int(nullable: false),
                        ExtraData = c.String(maxLength: 500),
                        Description = c.String(maxLength: 500),
                        ElapsedMilliseconds = c.Long(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoalPercent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        TouchedGoalPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EncouragePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.GoalGoalFulfillment", "GoalFulfillmentId", "dbo.GoalFulfillment");
            DropForeignKey("dbo.GoalGoalFulfillment", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.GoalFulfillment", "BranchId", "dbo.Branch");
            DropIndex("dbo.GoalGoalFulfillment", new[] { "GoalId" });
            DropIndex("dbo.GoalGoalFulfillment", new[] { "GoalFulfillmentId" });
            DropIndex("dbo.GoalFulfillment", new[] { "BranchId" });
            DropTable("dbo.GoalGoalFulfillment");
            DropTable("dbo.GoalFulfillment");
            CreateIndex("dbo.GoalPercent", "GoalId");
            AddForeignKey("dbo.GoalPercent", "GoalId", "dbo.Goal", "Id");
        }
    }
}
