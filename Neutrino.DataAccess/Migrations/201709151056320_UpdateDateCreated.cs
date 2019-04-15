namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDateCreated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppMenu", "DateCreated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppMenu", "DateCreated", c => c.DateTime());
        }
    }
}
