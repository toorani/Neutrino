namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsAllGoalGoods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoalGoodsCategory", "IsAllGoods", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoalGoodsCategory", "IsAllGoods");
        }
    }
}
