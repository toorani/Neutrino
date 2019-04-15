namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NationalCode = c.String(maxLength: 11),
                        Name = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 250),
                        Code = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        PositionId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        PositionType_eId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.PositionType", t => t.PositionType_eId)
                .Index(t => t.BranchId)
                .Index(t => t.PositionType_eId);
            
            CreateTable(
                "dbo.PositionType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 100),
                        Code = c.Int(),
                        Id1 = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Member", "PositionType_eId", "dbo.PositionType");
            DropForeignKey("dbo.Member", "BranchId", "dbo.Branch");
            DropIndex("dbo.Member", new[] { "PositionType_eId" });
            DropIndex("dbo.Member", new[] { "BranchId" });
            DropTable("dbo.PositionType");
            DropTable("dbo.Member");
        }
    }
}
