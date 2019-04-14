namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostCoefficient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostCoefficient",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsFormTypeId = c.Int(nullable: false),
                        Coefficient = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodsFormType", t => t.GoodsFormTypeId)
                .Index(t => t.GoodsFormTypeId);
            
            CreateTable(
                "dbo.GoodsFormType",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CostCoefficient", "GoodsFormTypeId", "dbo.GoodsFormType");
            DropIndex("dbo.CostCoefficient", new[] { "GoodsFormTypeId" });
            DropTable("dbo.GoodsFormType");
            DropTable("dbo.CostCoefficient");
        }
    }
}
