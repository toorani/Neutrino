namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterFulfillmentPercent_addfulfilledcolumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FulfillmentPercent", "TotalSalesFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "TotalReceiptFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FulfillmentPercent", "PrivateReceiptFulfilledPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FulfillmentPercent", "PrivateReceiptFulfilledPercent");
            DropColumn("dbo.FulfillmentPercent", "TotalReceiptFulfilledPercent");
            DropColumn("dbo.FulfillmentPercent", "TotalSalesFulfilledPercent");
        }
    }
}
