namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddGoalType_GoodsCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoodsCategory", "GoalTypeId", c => c.Int(nullable: true, defaultValue: 2));
            CreateIndex("dbo.GoodsCategory", "GoalTypeId");
            AddForeignKey("dbo.GoodsCategory", "GoalTypeId", "dbo.GoalType", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GoodsCategory", "GoalTypeId", "dbo.GoalType");
            DropIndex("dbo.GoodsCategory", new[] { "GoalTypeId" });
            DropColumn("dbo.GoodsCategory", "GoalTypeId");
        }
    }
}
