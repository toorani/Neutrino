namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFalgsToPromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotion", "IsSalesCalculated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Promotion", "IsReceiptCalculated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promotion", "IsReceiptCalculated");
            DropColumn("dbo.Promotion", "IsSalesCalculated");
        }
    }
}
