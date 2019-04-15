namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDrugScore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DrugScore",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartedStock = c.Int(nullable: false),
                        AddedStock = c.Int(nullable: false),
                        Sales = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        AddedStockSalesPercent = c.Decimal(nullable: false, precision: 6, scale: 3),
                        StartedStockSalesPercent = c.Decimal(nullable: false, precision: 6, scale: 3),
                        Score = c.Decimal(nullable: false, precision: 6, scale: 3),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .Index(t => t.CompanyId)
                .Index(t => t.GoodsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrugScore", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.DrugScore", "CompanyId", "dbo.Company");
            DropIndex("dbo.DrugScore", new[] { "GoodsId" });
            DropIndex("dbo.DrugScore", new[] { "CompanyId" });
            DropTable("dbo.DrugScore");
        }
    }
}
