namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMinMaxPromotion_OrgSturctShare : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrgStructureShare", "MinimumPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrgStructureShare", "MaximumPromotion", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrgStructureShare", "MaximumPromotion");
            DropColumn("dbo.OrgStructureShare", "MinimumPromotion");
        }
    }
}
