namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPostionMapping : DbMigration
    {
        public override void Up()
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
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "Group");
            DropTable("dbo.PostionMapping");
        }
    }
}
