namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_memberShare_memberPromotion : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SharePromotionType", newName: "StepPromotionType");
            DropForeignKey("dbo.MemberSharePromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropForeignKey("dbo.MemberSharePromotionDetail", "MemberSharePromotionId", "dbo.MemberSharePromotion");
            DropForeignKey("dbo.MemberSharePromotion", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberPenalty", "MemberSharePromotionId", "dbo.MemberSharePromotion");
            DropForeignKey("dbo.MemberPromotion", "GoalId", "dbo.Goal");
            DropIndex("dbo.MemberPenalty", new[] { "MemberSharePromotionId" });
            DropIndex("dbo.MemberSharePromotion", new[] { "BranchPromotionId" });
            DropIndex("dbo.MemberSharePromotion", new[] { "MemberId" });
            DropIndex("dbo.MemberSharePromotionDetail", new[] { "MemberSharePromotionId" });
            DropIndex("dbo.MemberPromotion", new[] { "GoalId" });
            CreateTable(
                "dbo.MemberPromotionDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberPromotionId = c.Int(nullable: false),
                        StepPromotionTypeId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        SupplierPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompensatoryPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BranchSalesPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberPromotion", t => t.MemberPromotionId)
                .Index(t => t.MemberPromotionId);
            
            CreateTable(
                "dbo.OperationPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchPromotionId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Promotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchPromotion", t => t.BranchPromotionId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.BranchPromotionId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.SellerPromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchPromotionId = c.Int(nullable: false),
                        GoalId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Promotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchPromotion", t => t.BranchPromotionId)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.BranchPromotionId)
                .Index(t => t.GoalId)
                .Index(t => t.MemberId);
            
            AddColumn("dbo.BranchPromotion", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MemberPenalty", "MemberPromotionId", c => c.Int(nullable: false));
            AddColumn("dbo.Member", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.MemberPromotion", "ManagerPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotion", "CEOPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotion", "FinalPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Promotion", "IsSupplierCalculated", c => c.Boolean(nullable: false));
            AddColumn("dbo.Promotion", "IsBranchSalesCalculated", c => c.Boolean(nullable: false));
            CreateIndex("dbo.MemberPenalty", "MemberPromotionId");
            AddForeignKey("dbo.MemberPenalty", "MemberPromotionId", "dbo.MemberPromotion", "Id");
            DropColumn("dbo.MemberPenalty", "MemberSharePromotionId");
            DropColumn("dbo.MemberPromotion", "GoalId");
            DropColumn("dbo.MemberPromotion", "Promotion");
            DropColumn("dbo.MemberPromotion", "Quantity");
            DropColumn("dbo.MemberPromotion", "Amount");
            DropColumn("dbo.Promotion", "IsSalesCalculated");
            DropTable("dbo.MemberSharePromotion");
            DropTable("dbo.MemberSharePromotionDetail");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MemberSharePromotionDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberSharePromotionId = c.Int(nullable: false),
                        SharePromotionTypeId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        BranchSalesPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompensatoryPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemberSharePromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchPromotionId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        ManagerPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CEOPromotion = c.Decimal(precision: 18, scale: 2),
                        FinalPromotion = c.Decimal(precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Promotion", "IsSalesCalculated", c => c.Boolean(nullable: false));
            AddColumn("dbo.MemberPromotion", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotion", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.MemberPromotion", "Promotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotion", "GoalId", c => c.Int(nullable: false));
            AddColumn("dbo.MemberPenalty", "MemberSharePromotionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SellerPromotion", "MemberId", "dbo.Member");
            DropForeignKey("dbo.SellerPromotion", "GoalId", "dbo.Goal");
            DropForeignKey("dbo.SellerPromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropForeignKey("dbo.OperationPromotion", "MemberId", "dbo.Member");
            DropForeignKey("dbo.OperationPromotion", "BranchPromotionId", "dbo.BranchPromotion");
            DropForeignKey("dbo.MemberPenalty", "MemberPromotionId", "dbo.MemberPromotion");
            DropForeignKey("dbo.MemberPromotionDetail", "MemberPromotionId", "dbo.MemberPromotion");
            DropIndex("dbo.SellerPromotion", new[] { "MemberId" });
            DropIndex("dbo.SellerPromotion", new[] { "GoalId" });
            DropIndex("dbo.SellerPromotion", new[] { "BranchPromotionId" });
            DropIndex("dbo.OperationPromotion", new[] { "MemberId" });
            DropIndex("dbo.OperationPromotion", new[] { "BranchPromotionId" });
            DropIndex("dbo.MemberPromotionDetail", new[] { "MemberPromotionId" });
            DropIndex("dbo.MemberPenalty", new[] { "MemberPromotionId" });
            DropColumn("dbo.Promotion", "IsBranchSalesCalculated");
            DropColumn("dbo.Promotion", "IsSupplierCalculated");
            DropColumn("dbo.MemberPromotion", "FinalPromotion");
            DropColumn("dbo.MemberPromotion", "CEOPromotion");
            DropColumn("dbo.MemberPromotion", "ManagerPromotion");
            DropColumn("dbo.Member", "IsActive");
            DropColumn("dbo.MemberPenalty", "MemberPromotionId");
            DropColumn("dbo.BranchPromotion", "Budget");
            DropTable("dbo.SellerPromotion");
            DropTable("dbo.OperationPromotion");
            DropTable("dbo.MemberPromotionDetail");
            CreateIndex("dbo.MemberPromotion", "GoalId");
            CreateIndex("dbo.MemberSharePromotionDetail", "MemberSharePromotionId");
            CreateIndex("dbo.MemberSharePromotion", "MemberId");
            CreateIndex("dbo.MemberSharePromotion", "BranchPromotionId");
            CreateIndex("dbo.MemberPenalty", "MemberSharePromotionId");
            AddForeignKey("dbo.MemberPromotion", "GoalId", "dbo.Goal", "Id");
            AddForeignKey("dbo.MemberPenalty", "MemberSharePromotionId", "dbo.MemberSharePromotion", "Id");
            AddForeignKey("dbo.MemberSharePromotion", "MemberId", "dbo.Member", "Id");
            AddForeignKey("dbo.MemberSharePromotionDetail", "MemberSharePromotionId", "dbo.MemberSharePromotion", "Id");
            AddForeignKey("dbo.MemberSharePromotion", "BranchPromotionId", "dbo.BranchPromotion", "Id");
            RenameTable(name: "dbo.StepPromotionType", newName: "SharePromotionType");
        }
    }
}
