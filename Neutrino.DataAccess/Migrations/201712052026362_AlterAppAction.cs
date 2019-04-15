namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAppAction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationAction", "HtmlUrl", c => c.String(maxLength: 200));
            AddColumn("dbo.ApplicationAction", "ServiceUrl", c => c.String(maxLength: 200));
            DropColumn("dbo.ApplicationAction", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationAction", "Url", c => c.String(maxLength: 200));
            DropColumn("dbo.ApplicationAction", "ServiceUrl");
            DropColumn("dbo.ApplicationAction", "HtmlUrl");
        }
    }
}
