namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoalStep", "IncrementPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GoalStep", "RawComputingValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GoalStep", "GoalTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoalStep", "GoalTypeId");
            AddForeignKey("dbo.GoalStep", "GoalTypeId", "dbo.GoalType", "Id");
            DropColumn("dbo.Goal", "IncrementPercent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goal", "IncrementPercent", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.GoalStep", "GoalTypeId", "dbo.GoalType");
            DropIndex("dbo.GoalStep", new[] { "GoalTypeId" });
            DropColumn("dbo.GoalStep", "GoalTypeId");
            DropColumn("dbo.GoalStep", "RawComputingValue");
            DropColumn("dbo.GoalStep", "IncrementPercent");
        }
    }
}
