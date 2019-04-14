namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGoalTypeInGoal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoalType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Goal", "SupplierGoalId", c => c.Int());
            AddColumn("dbo.Goal", "IncrementPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Goal", "GoalTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Goal", "SupplierGoalId");
            CreateIndex("dbo.Goal", "GoalTypeId");
            AddForeignKey("dbo.Goal", "GoalTypeId", "dbo.GoalType", "Id");
            AddForeignKey("dbo.Goal", "SupplierGoalId", "dbo.Goal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Goal", "SupplierGoalId", "dbo.Goal");
            DropForeignKey("dbo.Goal", "GoalTypeId", "dbo.GoalType");
            DropIndex("dbo.Goal", new[] { "GoalTypeId" });
            DropIndex("dbo.Goal", new[] { "SupplierGoalId" });
            DropColumn("dbo.Goal", "GoalTypeId");
            DropColumn("dbo.Goal", "IncrementPercent");
            DropColumn("dbo.Goal", "SupplierGoalId");
            DropTable("dbo.GoalType");
        }
    }
}
