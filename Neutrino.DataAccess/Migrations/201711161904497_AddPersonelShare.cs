namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonelShare : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonelShare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        OrgStructureId = c.Int(nullable: false),
                        SalePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceivablePercent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.OrgStructure", t => t.OrgStructureId)
                .Index(t => t.BranchId)
                .Index(t => t.OrgStructureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonelShare", "OrgStructureId", "dbo.OrgStructure");
            DropForeignKey("dbo.PersonelShare", "BranchId", "dbo.Branch");
            DropIndex("dbo.PersonelShare", new[] { "OrgStructureId" });
            DropIndex("dbo.PersonelShare", new[] { "BranchId" });
            DropTable("dbo.PersonelShare");
        }
    }
}
