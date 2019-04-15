namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQuantity_GoodsQuantityCondition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GoodsQuantityCondition", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GoodsQuantityCondition", "Quantity");
        }
    }
}
