namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMemebrPlenaty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberPenalty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        MemberSharePromotionId = c.Int(nullable: false),
                        BranchPromotionId = c.Int(nullable: false),
                        RemainingPenalty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Penalty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deduction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BranchPromotion", t => t.BranchPromotionId)
                .ForeignKey("dbo.MemberSharePromotion", t => t.MemberSharePromotionId)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.MemberId)
                .Index(t => t.MemberSharePromotionId)
                .Index(t => t.BranchPromotionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberPenalty", "MemberId", "dbo.Member");
            DropForeignKey("dbo.MemberPenalty", "MemberSharePromotionId", "dbo.MemberSharePromotion");
            DropForeignKey("dbo.MemberPenalty", "BranchPromotionId", "dbo.BranchPromotion");
            DropIndex("dbo.MemberPenalty", new[] { "BranchPromotionId" });
            DropIndex("dbo.MemberPenalty", new[] { "MemberSharePromotionId" });
            DropIndex("dbo.MemberPenalty", new[] { "MemberId" });
            DropTable("dbo.MemberPenalty");
        }
    }
}
