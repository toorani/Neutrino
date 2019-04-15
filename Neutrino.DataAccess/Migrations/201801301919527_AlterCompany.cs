namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "FaName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Company", "Code", c => c.String(maxLength: 20));
            AddColumn("dbo.Company", "RefId", c => c.Int(nullable: false));
            AlterColumn("dbo.Company", "EnName", c => c.String(maxLength: 50));
            DropColumn("dbo.Company", "Name");
            DropColumn("dbo.Company", "CompanyCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Company", "CompanyCode", c => c.String(maxLength: 20));
            AddColumn("dbo.Company", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Company", "EnName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Company", "RefId");
            DropColumn("dbo.Company", "Code");
            DropColumn("dbo.Company", "FaName");
        }
    }
}
