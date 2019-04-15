namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_AppMenu : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationMenu", newName: "AppMenu");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AppMenu", newName: "ApplicationMenu");
        }
    }
}
