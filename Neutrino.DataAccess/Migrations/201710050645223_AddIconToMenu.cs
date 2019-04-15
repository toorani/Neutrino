namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIconToMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppMenu", "Icon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppMenu", "Icon");
        }
    }
}
