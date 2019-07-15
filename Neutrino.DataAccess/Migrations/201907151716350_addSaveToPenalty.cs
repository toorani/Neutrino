namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSaveToPenalty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemberPenalty", "Saved", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemberPenalty", "Saved");
        }
    }
}
