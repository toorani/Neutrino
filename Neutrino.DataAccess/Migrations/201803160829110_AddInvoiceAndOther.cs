namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceAndOther : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchReceipt",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        BranchRefId = c.Int(nullable: false),
                        ReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.BranchSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        BranchRefId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        GoodsRefId = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .Index(t => t.BranchId)
                .Index(t => t.GoodsId);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SellerId = c.Int(nullable: false),
                        SellerRefId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        GoodsRefId = c.Int(nullable: false),
                        TotalCount = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .Index(t => t.GoodsId);
            
            CreateTable(
                "dbo.MemberReceipt",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        MemberRefId = c.Int(nullable: false),
                        ReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.MemberPayroll",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        MemberRefId = c.Int(nullable: false),
                        Payment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberPayroll", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberReceipt", "MemberId", "dbo.Member");
            DropForeignKey("dbo.Invoice", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.BranchSales", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.BranchSales", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.BranchReceipt", "BranchId", "dbo.Branch");
            DropIndex("dbo.MemberPayroll", new[] { "MemberId" });
            DropIndex("dbo.MemberReceipt", new[] { "MemberId" });
            DropIndex("dbo.Invoice", new[] { "GoodsId" });
            DropIndex("dbo.BranchSales", new[] { "GoodsId" });
            DropIndex("dbo.BranchSales", new[] { "BranchId" });
            DropIndex("dbo.BranchReceipt", new[] { "BranchId" });
            DropTable("dbo.MemberPayroll");
            DropTable("dbo.MemberReceipt");
            DropTable("dbo.Invoice");
            DropTable("dbo.BranchSales");
            DropTable("dbo.BranchReceipt");
        }
    }
}
