namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationAction", "ActionTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationAction", "ActionTypeId");
        }
    }
}
