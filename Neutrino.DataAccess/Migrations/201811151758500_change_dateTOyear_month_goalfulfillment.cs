namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_dateTOyear_month_goalfulfillment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoalFulfillment", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.GoalFulfillment", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.GoalFulfillment", "StartDate");
            DropColumn("dbo.GoalFulfillment", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalFulfillment", "EndDate", c => c.DateTime());
            AddColumn("dbo.GoalFulfillment", "StartDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.GoalFulfillment", "Year");
            DropColumn("dbo.GoalFulfillment", "Month");
        }
    }
}
