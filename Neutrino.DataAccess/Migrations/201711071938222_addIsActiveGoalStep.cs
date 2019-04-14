namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsActiveGoalStep : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoodsCategory", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goal", "IsUsed", c => c.Boolean(nullable: false));
            AddColumn("dbo.GoalStep", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoalStep", "IsActive");
            DropColumn("dbo.Goal", "IsUsed");
            DropColumn("dbo.GoodsCategory", "IsActive");
        }
    }
}
