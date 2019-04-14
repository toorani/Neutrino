namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserRole_Table : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserRole");
            CreateTable(
                "dbo.SupplierType",
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
            
            AddColumn("dbo.Branch", "Zone", c => c.Int(nullable: false));
            AddColumn("dbo.Branch", "Level", c => c.String(maxLength: 50));
            AddColumn("dbo.Company", "EnName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Goods", "OfficalCode", c => c.Int(nullable: false));
            AddColumn("dbo.Goods", "GenericCode", c => c.Int(nullable: false));
            AddColumn("dbo.Goods", "ProducerId", c => c.Int());
            AddColumn("dbo.Goods", "Brand", c => c.String(maxLength: 250));
            AddColumn("dbo.Goods", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Goods", "SalesPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Goods", "ConsumerPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Goods", "IsTechnicalApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goods", "SupplierTypeId", c => c.Int());
            AddColumn("dbo.Goods", "GoodsFormTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Goods", "HasTaxable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goods", "HasExtraCharge", c => c.Boolean(nullable: false));
            AddColumn("dbo.Goods", "PackageCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserRole", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.UserRole", "DateCreated", c => c.DateTime());
            AddColumn("dbo.UserRole", "CreatorID", c => c.Int());
            AddColumn("dbo.UserRole", "Deleted", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.UserRole", "Id");
            CreateIndex("dbo.Goods", "ProducerId");
            CreateIndex("dbo.Goods", "SupplierTypeId");
            CreateIndex("dbo.Goods", "GoodsFormTypeId");
            AddForeignKey("dbo.Goods", "GoodsFormTypeId", "dbo.GoodsFormType", "Id");
            AddForeignKey("dbo.Goods", "ProducerId", "dbo.Company", "Id");
            AddForeignKey("dbo.Goods", "SupplierTypeId", "dbo.SupplierType", "Id");
            DropColumn("dbo.Goods", "DosageId");
            DropColumn("dbo.Goods", "UsageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goods", "UsageId", c => c.Int());
            AddColumn("dbo.Goods", "DosageId", c => c.Int());
            DropForeignKey("dbo.Goods", "SupplierTypeId", "dbo.SupplierType");
            DropForeignKey("dbo.Goods", "ProducerId", "dbo.Company");
            DropForeignKey("dbo.Goods", "GoodsFormTypeId", "dbo.GoodsFormType");
            DropIndex("dbo.Goods", new[] { "GoodsFormTypeId" });
            DropIndex("dbo.Goods", new[] { "SupplierTypeId" });
            DropIndex("dbo.Goods", new[] { "ProducerId" });
            DropPrimaryKey("dbo.UserRole");
            DropColumn("dbo.UserRole", "Deleted");
            DropColumn("dbo.UserRole", "CreatorID");
            DropColumn("dbo.UserRole", "DateCreated");
            DropColumn("dbo.UserRole", "Id");
            DropColumn("dbo.Goods", "PackageCount");
            DropColumn("dbo.Goods", "HasExtraCharge");
            DropColumn("dbo.Goods", "HasTaxable");
            DropColumn("dbo.Goods", "GoodsFormTypeId");
            DropColumn("dbo.Goods", "SupplierTypeId");
            DropColumn("dbo.Goods", "IsTechnicalApproved");
            DropColumn("dbo.Goods", "ConsumerPrice");
            DropColumn("dbo.Goods", "SalesPrice");
            DropColumn("dbo.Goods", "PurchasePrice");
            DropColumn("dbo.Goods", "Brand");
            DropColumn("dbo.Goods", "ProducerId");
            DropColumn("dbo.Goods", "GenericCode");
            DropColumn("dbo.Goods", "OfficalCode");
            DropColumn("dbo.Company", "EnName");
            DropColumn("dbo.Branch", "Level");
            DropColumn("dbo.Branch", "Zone");
            DropTable("dbo.SupplierType");
            AddPrimaryKey("dbo.UserRole", new[] { "UserId", "RoleId" });
        }
    }
}
