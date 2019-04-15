namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPharmacyReport : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DrugScore", newName: "GoodsScore");
            CreateTable(
                "dbo.DrugGeneralName",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FaTitle = c.String(),
                        EnTitle = c.String(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoodsPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        SupplierPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalerPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistributorPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .Index(t => t.GoodsId);
            
            CreateTable(
                "dbo.TherapeuticType",
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
                "dbo.GoodsPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        GoodsPriceId = c.Int(nullable: false),
                        PromotionValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Eran = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .ForeignKey("dbo.GoodsPrice", t => t.GoodsPriceId)
                .Index(t => t.GoodsId)
                .Index(t => t.GoodsPriceId);
            
            AddColumn("dbo.Goods", "ATC", c => c.String());
            AddColumn("dbo.Goods", "TherapeuticTypeId", c => c.Int());
            AddColumn("dbo.Goods", "GeneralId", c => c.Int());
            AddColumn("dbo.Goods", "DrugGeneralName_Id", c => c.Int());
            CreateIndex("dbo.Goods", "TherapeuticTypeId");
            CreateIndex("dbo.Goods", "DrugGeneralName_Id");
            AddForeignKey("dbo.Goods", "DrugGeneralName_Id", "dbo.DrugGeneralName", "Id");
            AddForeignKey("dbo.Goods", "TherapeuticTypeId", "dbo.TherapeuticType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoodsPromotion", "GoodsPriceId", "dbo.GoodsPrice");
            DropForeignKey("dbo.GoodsPromotion", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.Goods", "TherapeuticTypeId", "dbo.TherapeuticType");
            DropForeignKey("dbo.GoodsPrice", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.Goods", "DrugGeneralName_Id", "dbo.DrugGeneralName");
            DropIndex("dbo.GoodsPromotion", new[] { "GoodsPriceId" });
            DropIndex("dbo.GoodsPromotion", new[] { "GoodsId" });
            DropIndex("dbo.GoodsPrice", new[] { "GoodsId" });
            DropIndex("dbo.Goods", new[] { "DrugGeneralName_Id" });
            DropIndex("dbo.Goods", new[] { "TherapeuticTypeId" });
            DropColumn("dbo.Goods", "DrugGeneralName_Id");
            DropColumn("dbo.Goods", "GeneralId");
            DropColumn("dbo.Goods", "TherapeuticTypeId");
            DropColumn("dbo.Goods", "ATC");
            DropTable("dbo.GoodsPromotion");
            DropTable("dbo.TherapeuticType");
            DropTable("dbo.GoodsPrice");
            DropTable("dbo.DrugGeneralName");
            RenameTable(name: "dbo.GoodsScore", newName: "DrugScore");
        }
    }
}
