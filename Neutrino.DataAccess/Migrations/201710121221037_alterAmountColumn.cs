namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterAmountColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GoalStepItemInfo", "Amount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GoalStepItemInfo", "Amount", c => c.Single());
        }
    }
}
