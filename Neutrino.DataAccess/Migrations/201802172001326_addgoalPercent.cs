namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addgoalPercent : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.GoalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalPercent", "GoalId", "dbo.Goal");
            DropIndex("dbo.GoalPercent", new[] { "GoalId" });
            DropTable("dbo.GoalPercent");
        }
    }
}
