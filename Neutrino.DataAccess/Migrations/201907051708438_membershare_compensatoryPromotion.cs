namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class membershare_compensatoryPromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberSharePromotionDetail", "CompensatoryPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MemberSharePromotionDetail", "SellerPromotion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberSharePromotionDetail", "SellerPromotion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MemberSharePromotionDetail", "CompensatoryPromotion");
        }
    }
}
