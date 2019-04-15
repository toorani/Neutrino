namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_remove_isactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoalFulfillment", "IsUsed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Goal", "IsActive");
            DropColumn("dbo.GoalFulfillment", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalFulfillment", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goal", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.GoalFulfillment", "IsUsed");
        }
    }
}
