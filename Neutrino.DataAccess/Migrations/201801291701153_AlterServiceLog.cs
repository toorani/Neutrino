namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterServiceLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceLog", "ExtraData", c => c.String(maxLength: 500));
            AddColumn("dbo.ServiceLog", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceLog", "Description");
            DropColumn("dbo.ServiceLog", "ExtraData");
        }
    }
}
