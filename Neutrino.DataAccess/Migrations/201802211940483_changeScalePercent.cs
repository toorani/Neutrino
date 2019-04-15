namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeScalePercent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BranchBenefit", "Percent", c => c.Decimal(nullable: false, precision: 9, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BranchBenefit", "Percent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
