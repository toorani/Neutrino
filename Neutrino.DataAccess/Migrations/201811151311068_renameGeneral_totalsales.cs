namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameGeneral_totalsales : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GeneralGoalRange", "GoalId", "dbo.Goal");
            DropIndex("dbo.GeneralGoalRange", new[] { "GoalId" });
            CreateTable(
                "dbo.TotalSalesGoalRange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        StartTotalSalesRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartReceiptRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionTotalSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionReceipt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goal", t => t.GoalId)
                .Index(t => t.GoalId);
            
            DropTable("dbo.GeneralGoalRange");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GeneralGoalRange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        StartGeneralRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartReceiptRange = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionGeneral = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionReceipt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.TotalSalesGoalRange", "GoalId", "dbo.Goal");
            DropIndex("dbo.TotalSalesGoalRange", new[] { "GoalId" });
            DropTable("dbo.TotalSalesGoalRange");
            CreateIndex("dbo.GeneralGoalRange", "GoalId");
            AddForeignKey("dbo.GeneralGoalRange", "GoalId", "dbo.Goal", "Id");
        }
    }
}
