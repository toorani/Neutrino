namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterOrgShare_addPrivateReceipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrgStructureShare", "SalesPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrgStructureShare", "PrivateReceiptPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrgStructureShare", "TotalReceiptPercent", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.OrgStructureShare", "SalePercent");
            DropColumn("dbo.OrgStructureShare", "ReceivablePercent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrgStructureShare", "ReceivablePercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.OrgStructureShare", "SalePercent", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.OrgStructureShare", "TotalReceiptPercent");
            DropColumn("dbo.OrgStructureShare", "PrivateReceiptPercent");
            DropColumn("dbo.OrgStructureShare", "SalesPercent");
        }
    }
}
