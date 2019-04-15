namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBenefit_Branch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchBenefit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Branch",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 300),
                        CityName = c.String(maxLength: 50),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BranchBenefitGoodsCategory",
                c => new
                    {
                        BranchBenefitId = c.Int(nullable: false),
                        GoodsCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchBenefitId, t.GoodsCategoryId })
                .ForeignKey("dbo.BranchBenefit", t => t.BranchBenefitId)
                .ForeignKey("dbo.GoodsCategory", t => t.GoodsCategoryId)
                .Index(t => t.BranchBenefitId)
                .Index(t => t.GoodsCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchBenefitGoodsCategory", "GoodsCategoryId", "dbo.GoodsCategory");
            DropForeignKey("dbo.BranchBenefitGoodsCategory", "BranchBenefitId", "dbo.BranchBenefit");
            DropForeignKey("dbo.BranchBenefit", "BranchId", "dbo.Branch");
            DropIndex("dbo.BranchBenefitGoodsCategory", new[] { "GoodsCategoryId" });
            DropIndex("dbo.BranchBenefitGoodsCategory", new[] { "BranchBenefitId" });
            DropIndex("dbo.BranchBenefit", new[] { "BranchId" });
            DropTable("dbo.BranchBenefitGoodsCategory");
            DropTable("dbo.Branch");
            DropTable("dbo.BranchBenefit");
        }
    }
}
