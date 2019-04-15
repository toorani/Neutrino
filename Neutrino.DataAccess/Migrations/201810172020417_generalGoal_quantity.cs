namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class generalGoal_quantity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DrugGeneralName", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.SalesLedger", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.SalesLedger", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.UploadedFile", "CompanyId", "dbo.Company");
            DropIndex("dbo.GoodsQuantityCondition", new[] { "QuantityCondition_Id" });
            DropIndex("dbo.DrugGeneralName", new[] { "GoodsId" });
            DropIndex("dbo.SalesLedger", new[] { "GoodsId" });
            DropIndex("dbo.SalesLedger", new[] { "CompanyId" });
            DropIndex("dbo.UploadedFile", new[] { "CompanyId" });
            RenameColumn(table: "dbo.GoodsQuantityCondition", name: "QuantityCondition_Id", newName: "QuantityConditionId");
            CreateTable(
                "dbo.GeneralGoalRange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        StartRangePrecent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EndRangePrecent = c.Decimal(precision: 18, scale: 2),
                        CommissionPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.GoalId);
            
            AddColumn("dbo.QuantityCondition", "NotReachedPercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.QuantityCondition", "ForthCasePercent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GoodsQuantityCondition", "QuantityConditionId", c => c.Int(nullable: false));
            CreateIndex("dbo.GoodsQuantityCondition", "QuantityConditionId");
            DropTable("dbo.DrugGeneralName");
            DropTable("dbo.SalesLedger");
            DropTable("dbo.UploadedFile");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UploadedFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OriginalFileName = c.String(),
                        UploadedFileName = c.String(),
                        HashValue = c.String(),
                        CompanyId = c.Int(nullable: false),
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
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        CompanyId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DrugGeneralName",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FaTitle = c.String(),
                        EnTitle = c.String(),
                        GoodsId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.GeneralGoalRange", "GoalId", "dbo.Goal");
            DropIndex("dbo.GeneralGoalRange", new[] { "GoalId" });
            DropIndex("dbo.GoodsQuantityCondition", new[] { "QuantityConditionId" });
            AlterColumn("dbo.GoodsQuantityCondition", "QuantityConditionId", c => c.Int());
            DropColumn("dbo.QuantityCondition", "ForthCasePercent");
            DropColumn("dbo.QuantityCondition", "NotReachedPercent");
            DropTable("dbo.GeneralGoalRange");
            RenameColumn(table: "dbo.GoodsQuantityCondition", name: "QuantityConditionId", newName: "QuantityCondition_Id");
            CreateIndex("dbo.UploadedFile", "CompanyId");
            CreateIndex("dbo.SalesLedger", "CompanyId");
            CreateIndex("dbo.SalesLedger", "GoodsId");
            CreateIndex("dbo.DrugGeneralName", "GoodsId");
            CreateIndex("dbo.GoodsQuantityCondition", "QuantityCondition_Id");
            AddForeignKey("dbo.UploadedFile", "CompanyId", "dbo.Company", "Id");
            AddForeignKey("dbo.SalesLedger", "GoodsId", "dbo.Goods", "Id");
            AddForeignKey("dbo.SalesLedger", "CompanyId", "dbo.Company", "Id");
            AddForeignKey("dbo.DrugGeneralName", "GoodsId", "dbo.Goods", "Id");
        }
    }
}
