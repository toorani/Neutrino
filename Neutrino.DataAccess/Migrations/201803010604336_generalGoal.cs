namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class generalGoal : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GoalGoodsCategory", "IsAllGoods");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GoalGoodsCategory", "IsAllGoods", c => c.Boolean(nullable: false));
        }
    }
}
