namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addInvoiceDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoice", "InvoiceDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoice", "InvoiceDate");
        }
    }
}
