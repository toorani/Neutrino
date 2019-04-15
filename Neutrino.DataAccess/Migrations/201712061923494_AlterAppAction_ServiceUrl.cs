namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAppAction_ServiceUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationAction", "ActionUrl", c => c.String(maxLength: 200));
            DropColumn("dbo.ApplicationAction", "ServiceUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationAction", "ServiceUrl", c => c.String(maxLength: 200));
            DropColumn("dbo.ApplicationAction", "ActionUrl");
        }
    }
}
