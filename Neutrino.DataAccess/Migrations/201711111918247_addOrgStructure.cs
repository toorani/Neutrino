namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrgStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrgStructure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        Title = c.String(),
                        Code = c.String(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .Index(t => t.BranchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgStructure", "BranchId", "dbo.Branch");
            DropIndex("dbo.OrgStructure", new[] { "BranchId" });
            DropTable("dbo.OrgStructure");
        }
    }
}
