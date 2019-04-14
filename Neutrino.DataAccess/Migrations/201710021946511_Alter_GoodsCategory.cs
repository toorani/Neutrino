namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_GoodsCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRole", "User_Id", "dbo.User");
            DropForeignKey("dbo.UserRole", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            CreateTable(
                "dbo.ComputingType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CondemnationType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Goal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(),
                        GoodsCategoryId = c.Int(nullable: false),
                        GoodsCategoryTypeId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .ForeignKey("dbo.GoodsCategory", t => t.GoodsCategoryId)
                .ForeignKey("dbo.GoodsCategoryType", t => t.GoodsCategoryTypeId)
                .Index(t => t.CompanyId)
                .Index(t => t.GoodsCategoryId)
                .Index(t => t.GoodsCategoryTypeId);
            
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CompanyCode = c.String(maxLength: 20),
                        NationalCode = c.String(maxLength: 11),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoalStep",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        ComputingTypeId = c.Int(nullable: false),
                        RewardTypeId = c.Int(),
                        CondemnationTypeId = c.Int(),
                        SellerRewardInfoId = c.Int(),
                        InvoiceRewardInfoId = c.Int(),
                        OtherRewardId = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComputingType", t => t.ComputingTypeId)
                .ForeignKey("dbo.CondemnationType", t => t.CondemnationTypeId)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .ForeignKey("dbo.RewardItemInfo", t => t.InvoiceRewardInfoId)
                .ForeignKey("dbo.OtherRewardType", t => t.OtherRewardId)
                .ForeignKey("dbo.RewardType", t => t.RewardTypeId)
                .ForeignKey("dbo.RewardItemInfo", t => t.SellerRewardInfoId)
                .Index(t => t.GoalId)
                .Index(t => t.ComputingTypeId)
                .Index(t => t.RewardTypeId)
                .Index(t => t.CondemnationTypeId)
                .Index(t => t.SellerRewardInfoId)
                .Index(t => t.InvoiceRewardInfoId)
                .Index(t => t.OtherRewardId);
            
            CreateTable(
                "dbo.RewardItemInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComputingTypeId = c.Int(nullable: false),
                        ItemValue = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComputingType", t => t.ComputingTypeId)
                .Index(t => t.ComputingTypeId);
            
            CreateTable(
                "dbo.OtherRewardType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RewardType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoodsCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        GoodsCategoryTypeId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodsCategoryType", t => t.GoodsCategoryTypeId)
                .Index(t => t.GoodsCategoryTypeId);
            
            CreateTable(
                "dbo.GoodsCategoryType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnName = c.String(nullable: false, maxLength: 256),
                        FaName = c.String(nullable: false, maxLength: 256),
                        GoodsCode = c.String(maxLength: 20),
                        CompanyId = c.Int(nullable: false),
                        StatusId = c.Short(),
                        DosageId = c.Int(),
                        UsageId = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.GoodsGoodsCategory",
                c => new
                    {
                        Goods_Id = c.Int(nullable: false),
                        GoodsCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Goods_Id, t.GoodsCategory_Id })
                .ForeignKey("dbo.Goods", t => t.Goods_Id)
                .ForeignKey("dbo.GoodsCategory", t => t.GoodsCategory_Id)
                .Index(t => t.Goods_Id)
                .Index(t => t.GoodsCategory_Id);
            
            AddForeignKey("dbo.UserRole", "User_Id", "dbo.User", "Id");
            AddForeignKey("dbo.UserRole", "Role_Id", "dbo.Role", "Id");
            AddForeignKey("dbo.UserClaim", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.UserLogin", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.UserRole", "User_Id", "dbo.User");
            DropForeignKey("dbo.Goal", "GoodsCategoryTypeId", "dbo.GoodsCategoryType");
            DropForeignKey("dbo.GoodsGoodsCategory", "GoodsCategory_Id", "dbo.GoodsCategory");
            DropForeignKey("dbo.GoodsGoodsCategory", "Goods_Id", "dbo.Goods");
            DropForeignKey("dbo.Goods", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.GoodsCategory", "GoodsCategoryTypeId", "dbo.GoodsCategoryType");
            DropForeignKey("dbo.Goal", "GoodsCategoryId", "dbo.GoodsCategory");
            DropForeignKey("dbo.GoalStep", "SellerRewardInfoId", "dbo.RewardItemInfo");
            DropForeignKey("dbo.GoalStep", "RewardTypeId", "dbo.RewardType");
            DropForeignKey("dbo.GoalStep", "OtherRewardId", "dbo.OtherRewardType");
            DropForeignKey("dbo.GoalStep", "InvoiceRewardInfoId", "dbo.RewardItemInfo");
            DropForeignKey("dbo.RewardItemInfo", "ComputingTypeId", "dbo.ComputingType");
            DropForeignKey("dbo.GoalStep", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.GoalStep", "CondemnationTypeId", "dbo.CondemnationType");
            DropForeignKey("dbo.GoalStep", "ComputingTypeId", "dbo.ComputingType");
            DropForeignKey("dbo.Goal", "CompanyId", "dbo.Company");
            DropIndex("dbo.GoodsGoodsCategory", new[] { "GoodsCategory_Id" });
            DropIndex("dbo.GoodsGoodsCategory", new[] { "Goods_Id" });
            DropIndex("dbo.Goods", new[] { "CompanyId" });
            DropIndex("dbo.GoodsCategory", new[] { "GoodsCategoryTypeId" });
            DropIndex("dbo.RewardItemInfo", new[] { "ComputingTypeId" });
            DropIndex("dbo.GoalStep", new[] { "OtherRewardId" });
            DropIndex("dbo.GoalStep", new[] { "InvoiceRewardInfoId" });
            DropIndex("dbo.GoalStep", new[] { "SellerRewardInfoId" });
            DropIndex("dbo.GoalStep", new[] { "CondemnationTypeId" });
            DropIndex("dbo.GoalStep", new[] { "RewardTypeId" });
            DropIndex("dbo.GoalStep", new[] { "ComputingTypeId" });
            DropIndex("dbo.GoalStep", new[] { "GoalId" });
            DropIndex("dbo.Goal", new[] { "GoodsCategoryTypeId" });
            DropIndex("dbo.Goal", new[] { "GoodsCategoryId" });
            DropIndex("dbo.Goal", new[] { "CompanyId" });
            DropTable("dbo.GoodsGoodsCategory");
            DropTable("dbo.Goods");
            DropTable("dbo.GoodsCategoryType");
            DropTable("dbo.GoodsCategory");
            DropTable("dbo.RewardType");
            DropTable("dbo.OtherRewardType");
            DropTable("dbo.RewardItemInfo");
            DropTable("dbo.GoalStep");
            DropTable("dbo.Company");
            DropTable("dbo.Goal");
            DropTable("dbo.CondemnationType");
            DropTable("dbo.ComputingType");
            AddForeignKey("dbo.UserLogin", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserClaim", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRole", "Role_Id", "dbo.Role", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRole", "User_Id", "dbo.User", "Id", cascadeDelete: true);
        }
    }
}
