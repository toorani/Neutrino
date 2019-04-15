namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Receipt_General_Amount_Goal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Goal", "GeneralAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Goal", "ReceiptAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GeneralGoalRange", "StartGeneralRange", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneralGoalRange", "StartReceiptRange", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneralGoalRange", "CommissionGeneral", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneralGoalRange", "CommissionReceipt", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.GeneralGoalRange", "StartRangePrecent");
            DropColumn("dbo.GeneralGoalRange", "EndRangePrecent");
            DropColumn("dbo.GeneralGoalRange", "CommissionPercent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneralGoalRange", "CommissionPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GeneralGoalRange", "EndRangePrecent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GeneralGoalRange", "StartRangePrecent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.GeneralGoalRange", "CommissionReceipt");
            DropColumn("dbo.GeneralGoalRange", "CommissionGeneral");
            DropColumn("dbo.GeneralGoalRange", "StartReceiptRange");
            DropColumn("dbo.GeneralGoalRange", "StartGeneralRange");
            DropColumn("dbo.Goal", "ReceiptAmount");
            DropColumn("dbo.Goal", "GeneralAmount");
        }
    }
}
