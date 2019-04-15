namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterDb : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.QuantityCondition", new[] { "QuantityConditionTypeId" });
            AlterColumn("dbo.QuantityCondition", "QuantityConditionTypeId", c => c.Int());
            CreateIndex("dbo.QuantityCondition", "QuantityConditionTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.QuantityCondition", new[] { "QuantityConditionTypeId" });
            AlterColumn("dbo.QuantityCondition", "QuantityConditionTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.QuantityCondition", "QuantityConditionTypeId");
        }
    }
}
