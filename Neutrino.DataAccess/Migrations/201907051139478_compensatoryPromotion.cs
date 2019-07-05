namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class compensatoryPromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchPromotion", "CompensatoryPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BranchPromotion", "CompensatoryPromotion");
        }
    }
}
