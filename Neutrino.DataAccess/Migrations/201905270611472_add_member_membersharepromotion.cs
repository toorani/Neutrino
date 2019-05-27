namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_member_membersharepromotion : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.MemberSharePromotion", "MemberId");
            AddForeignKey("dbo.MemberSharePromotion", "MemberId", "dbo.Member", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberSharePromotion", "MemberId", "dbo.Member");
            DropIndex("dbo.MemberSharePromotion", new[] { "MemberId" });
        }
    }
}
