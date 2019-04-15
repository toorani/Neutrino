namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterBranchReciept : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BranchReceipt", "PrivateAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BranchReceipt", "GovernmentalAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BranchReceipt", "TotalAmount", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.BranchReceipt", "ReceiptAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BranchReceipt", "ReceiptAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BranchReceipt", "TotalAmount");
            DropColumn("dbo.BranchReceipt", "GovernmentalAmount");
            DropColumn("dbo.BranchReceipt", "PrivateAmount");
        }
    }
}
