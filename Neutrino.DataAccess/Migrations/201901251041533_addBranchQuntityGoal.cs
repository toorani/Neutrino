namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBranchQuntityGoal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BranchQuntityGoal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        TotalNumber = c.Double(nullable: false),
                        Quntity = c.Int(nullable: false),
                        IsTouchTarget = c.Boolean(nullable: false),
                        TouchingPercent = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BranchQuntityGoal");
        }
    }
}
