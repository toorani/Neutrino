namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterApplicationAction_removeparentid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationAction", "ParentId", "dbo.ApplicationAction");
            DropIndex("dbo.ApplicationAction", new[] { "ParentId" });
            AlterColumn("dbo.ApplicationAction", "HtmlUrl", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ApplicationAction", "ActionUrl", c => c.String(maxLength: 200));
            DropColumn("dbo.ApplicationAction", "Title");
            DropColumn("dbo.ApplicationAction", "FaTitle");
            DropColumn("dbo.ApplicationAction", "ParentId");
            DropColumn("dbo.ApplicationAction", "ActionTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationAction", "ActionTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationAction", "ParentId", c => c.Int());
            AddColumn("dbo.ApplicationAction", "FaTitle", c => c.String(maxLength: 100));
            AddColumn("dbo.ApplicationAction", "Title", c => c.String(maxLength: 100));
            AlterColumn("dbo.ApplicationAction", "ActionUrl", c => c.String(maxLength: 200));
            AlterColumn("dbo.ApplicationAction", "HtmlUrl", c => c.String(maxLength: 200));
            CreateIndex("dbo.ApplicationAction", "ParentId");
            AddForeignKey("dbo.ApplicationAction", "ParentId", "dbo.ApplicationAction", "Id");
        }
    }
}
