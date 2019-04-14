namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermisson : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        ApplicationActionId = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationAction", t => t.ApplicationActionId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.ApplicationActionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Permission", "ApplicationActionId", "dbo.ApplicationAction");
            DropIndex("dbo.Permission", new[] { "ApplicationActionId" });
            DropIndex("dbo.Permission", new[] { "RoleId" });
            DropTable("dbo.Permission");
        }
    }
}
