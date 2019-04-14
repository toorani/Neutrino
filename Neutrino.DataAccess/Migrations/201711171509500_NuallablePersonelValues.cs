namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NuallablePersonelValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PersonelShare", "SalePercent", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PersonelShare", "ReceivablePercent", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonelShare", "ReceivablePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PersonelShare", "SalePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
