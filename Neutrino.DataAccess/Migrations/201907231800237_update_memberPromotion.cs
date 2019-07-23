namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_memberPromotion : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StepPromotionType", newName: "ReviewPromotionStep");
            AddColumn("dbo.MemberPromotion", "Promotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotionDetail", "ReviewPromotionStepId", c => c.Int(nullable: false));
            DropColumn("dbo.MemberPromotion", "ManagerPromotion");
            DropColumn("dbo.MemberPromotion", "CEOPromotion");
            DropColumn("dbo.MemberPromotion", "FinalPromotion");
            DropColumn("dbo.MemberPromotionDetail", "StepPromotionTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberPromotionDetail", "StepPromotionTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.MemberPromotion", "FinalPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotion", "CEOPromotion", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.MemberPromotion", "ManagerPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MemberPromotionDetail", "ReviewPromotionStepId");
            DropColumn("dbo.MemberPromotion", "Promotion");
            RenameTable(name: "dbo.ReviewPromotionStep", newName: "StepPromotionType");
        }
    }
}
