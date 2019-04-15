namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVisibleToGoalCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoalGoodsCategory", "IsVisible", c => c.Boolean(nullable: false,defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoalGoodsCategory", "IsVisible");
        }
    }
}
