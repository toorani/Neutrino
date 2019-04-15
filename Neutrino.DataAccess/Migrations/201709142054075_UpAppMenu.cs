namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpAppMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppMenu", "ParentId", c => c.Int());
            AddColumn("dbo.AppMenu", "DateCreated", c => c.DateTime());
            AddColumn("dbo.AppMenu", "Deleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AppMenu", "Title", c => c.String(maxLength: 256));
            AlterColumn("dbo.AppMenu", "Url", c => c.String(maxLength: 256));
            CreateIndex("dbo.AppMenu", "ParentId");
            AddForeignKey("dbo.AppMenu", "ParentId", "dbo.AppMenu", "Id");
            DropColumn("dbo.AppMenu", "CreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppMenu", "CreateDate", c => c.DateTime());
            DropForeignKey("dbo.AppMenu", "ParentId", "dbo.AppMenu");
            DropIndex("dbo.AppMenu", new[] { "ParentId" });
            AlterColumn("dbo.AppMenu", "Url", c => c.String());
            AlterColumn("dbo.AppMenu", "Title", c => c.Int(nullable: false));
            DropColumn("dbo.AppMenu", "Deleted");
            DropColumn("dbo.AppMenu", "DateCreated");
            DropColumn("dbo.AppMenu", "ParentId");
        }
    }
}
