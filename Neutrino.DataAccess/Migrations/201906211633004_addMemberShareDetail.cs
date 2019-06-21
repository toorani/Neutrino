namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMemberShareDetail : DbMigration
    {
        public override void Up()
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
                        SellerPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptPromotion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberSharePromotion", t => t.MemberSharePromotionId)
                .Index(t => t.MemberSharePromotionId);
            
            CreateTable(
                "dbo.SharePromotionType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.MemberPromotion", "MemberId");
            AddForeignKey("dbo.MemberPromotion", "MemberId", "dbo.Member", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberPromotion", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberSharePromotionDetail", "MemberSharePromotionId", "dbo.MemberSharePromotion");
            DropIndex("dbo.MemberPromotion", new[] { "MemberId" });
            DropIndex("dbo.MemberSharePromotionDetail", new[] { "MemberSharePromotionId" });
            DropTable("dbo.SharePromotionType");
            DropTable("dbo.MemberSharePromotionDetail");
        }
    }
}
