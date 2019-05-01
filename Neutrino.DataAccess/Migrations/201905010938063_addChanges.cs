namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChanges : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.BranchPromotion", "BranchId");
            AddForeignKey("dbo.BranchPromotion", "BranchId", "dbo.Branch", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BranchPromotion", "BranchId", "dbo.Branch");
            DropIndex("dbo.BranchPromotion", new[] { "BranchId" });
        }
    }
}
