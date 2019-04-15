namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSalesAndStock : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesLedger",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .Index(t => t.GoodsId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.StockInventory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .Index(t => t.GoodsId)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.Company", "CompanyTypeId", c => c.Int());
            CreateIndex("dbo.Company", "CompanyTypeId");
            AddForeignKey("dbo.Company", "CompanyTypeId", "dbo.CompanyType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockInventory", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.StockInventory", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.SalesLedger", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.SalesLedger", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "CompanyTypeId", "dbo.CompanyType");
            DropIndex("dbo.StockInventory", new[] { "CompanyId" });
            DropIndex("dbo.StockInventory", new[] { "GoodsId" });
            DropIndex("dbo.SalesLedger", new[] { "CompanyId" });
            DropIndex("dbo.SalesLedger", new[] { "GoodsId" });
            DropIndex("dbo.Company", new[] { "CompanyTypeId" });
            DropColumn("dbo.Company", "CompanyTypeId");
            DropTable("dbo.StockInventory");
            DropTable("dbo.SalesLedger");
            DropTable("dbo.CompanyType");
        }
    }
}
