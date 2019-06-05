namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addelitePosition_department : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefId = c.Int(nullable: false),
                        Title = c.String(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ElitePosition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefId = c.Int(nullable: false),
                        Title = c.String(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PositionMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostionRefId = c.Int(nullable: false),
                        ElitePositionId = c.Int(nullable: false),
                        PositionTypeId = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Member", "DepartmentRefId", c => c.Int(nullable: false));
            AddColumn("dbo.Member", "DepartmentId", c => c.Int(nullable: false));
            DropColumn("dbo.Member", "Group");
            DropTable("dbo.PostionMapping");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PostionMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        PostionRefId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        BranchRefId = c.Int(nullable: false),
                        PositionTypeId = c.Int(),
                        DateCreated = c.DateTime(),
                        CreatorID = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(),
                        EditorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Member", "Group", c => c.Int());
            DropColumn("dbo.Member", "DepartmentId");
            DropColumn("dbo.Member", "DepartmentRefId");
            DropTable("dbo.PositionMapping");
            DropTable("dbo.ElitePosition");
            DropTable("dbo.Department");
        }
    }
}
