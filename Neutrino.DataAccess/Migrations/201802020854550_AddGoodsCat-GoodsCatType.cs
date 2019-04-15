namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGoodsCatGoodsCatType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Goods", "GoodsFormTypeId", "dbo.GoodsFormType");
            DropForeignKey("dbo.CostCoefficient", "GoodsFormTypeId", "dbo.GoodsFormType");
            DropIndex("dbo.Goods", new[] { "GoodsFormTypeId" });
            DropIndex("dbo.CostCoefficient", new[] { "GoodsFormTypeId" });
            CreateTable(
                "dbo.GoodsCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsId = c.Int(nullable: false),
                        GoodsCatgoryTypeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        GoodsCategoryType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId)
                .ForeignKey("dbo.GoodsCategoryType", t => t.GoodsCategoryType_Id)
                .Index(t => t.GoodsId)
                .Index(t => t.GoodsCategoryType_Id);
            
            CreateTable(
                "dbo.GoodsCategoryType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CostCoefficient", "GoodsCategoryTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.CostCoefficient", "GoodsCategoryTypeId");
            AddForeignKey("dbo.CostCoefficient", "GoodsCategoryTypeId", "dbo.GoodsCategoryType", "Id");
            DropColumn("dbo.Goods", "GoodsFormTypeId");
            DropColumn("dbo.CostCoefficient", "GoodsFormTypeId");
            DropTable("dbo.GoodsFormType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GoodsFormType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        Id1 = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CostCoefficient", "GoodsFormTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Goods", "GoodsFormTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CostCoefficient", "GoodsCategoryTypeId", "dbo.GoodsCategoryType");
            DropForeignKey("dbo.GoodsCategory", "GoodsCategoryType_Id", "dbo.GoodsCategoryType");
            DropForeignKey("dbo.GoodsCategory", "GoodsId", "dbo.Goods");
            DropIndex("dbo.CostCoefficient", new[] { "GoodsCategoryTypeId" });
            DropIndex("dbo.GoodsCategory", new[] { "GoodsCategoryType_Id" });
            DropIndex("dbo.GoodsCategory", new[] { "GoodsId" });
            DropColumn("dbo.CostCoefficient", "GoodsCategoryTypeId");
            DropTable("dbo.GoodsCategoryType");
            DropTable("dbo.GoodsCategory");
            CreateIndex("dbo.CostCoefficient", "GoodsFormTypeId");
            CreateIndex("dbo.Goods", "GoodsFormTypeId");
            AddForeignKey("dbo.CostCoefficient", "GoodsFormTypeId", "dbo.GoodsFormType", "Id");
            AddForeignKey("dbo.Goods", "GoodsFormTypeId", "dbo.GoodsFormType", "Id");
        }
    }
}
