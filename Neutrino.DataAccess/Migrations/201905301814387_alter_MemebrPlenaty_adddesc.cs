namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_MemebrPlenaty_adddesc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberPenalty", "Description", c => c.String(maxLength: 800));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberPenalty", "Description");
        }
    }
}
