namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addQau_amount_memberPer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberPromotion", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.MemberPromotion", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.MemberPromotion", "GoalId");
            AddForeignKey("dbo.MemberPromotion", "GoalId", "dbo.Goal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberPromotion", "GoalId", "dbo.Goal");
            DropIndex("dbo.MemberPromotion", new[] { "GoalId" });
            DropColumn("dbo.MemberPromotion", "Amount");
            DropColumn("dbo.MemberPromotion", "Quantity");
        }
    }
}
