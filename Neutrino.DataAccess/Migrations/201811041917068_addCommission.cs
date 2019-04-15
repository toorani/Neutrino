namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCommission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CommissionStatusId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommissionStatus", t => t.CommissionStatusId)
                .Index(t => t.CommissionStatusId);
            
            CreateTable(
                "dbo.CommissionStatus",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Goal", "Commission_Id", c => c.Int());
            CreateIndex("dbo.Goal", "Commission_Id");
            AddForeignKey("dbo.Goal", "Commission_Id", "dbo.Commission", "Id");
            DropColumn("dbo.Goal", "GeneralAmount");
            DropColumn("dbo.Goal", "ReceiptAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Goal", "ReceiptAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Goal", "GeneralAmount", c => c.Decimal(precision: 18, scale: 2));
            DropForeignKey("dbo.Commission", "CommissionStatusId", "dbo.CommissionStatus");
            DropForeignKey("dbo.Goal", "Commission_Id", "dbo.Commission");
            DropIndex("dbo.Commission", new[] { "CommissionStatusId" });
            DropIndex("dbo.Goal", new[] { "Commission_Id" });
            DropColumn("dbo.Goal", "Commission_Id");
            DropTable("dbo.CommissionStatus");
            DropTable("dbo.Commission");
        }
    }
}
