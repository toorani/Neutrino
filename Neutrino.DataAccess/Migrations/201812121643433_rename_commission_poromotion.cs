namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_commission_poromotion : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Commission", newName: "Promotion");
            RenameTable(name: "dbo.CommissionStatus", newName: "PromotionStatus");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PromotionStatus", newName: "CommissionStatus");
            RenameTable(name: "dbo.Promotion", newName: "Commission");
        }
    }
}
