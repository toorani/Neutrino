namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createNeutrinoUserRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Role", "FaName", c => c.String(maxLength: 256));
            AddColumn("dbo.Role", "IsUsingBySystem", c => c.Boolean());
         
        }
        
        public override void Down()
        {
            DropColumn("dbo.Role", "IsUsingBySystem");
            DropColumn("dbo.Role", "FaName");
        }
    }
}
