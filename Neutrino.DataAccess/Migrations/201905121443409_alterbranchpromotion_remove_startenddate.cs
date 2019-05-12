namespace Neutrino.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterbranchpromotion_remove_startenddate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationAction", "ActionUrl", c => c.String(maxLength: 200));
            DropColumn("dbo.BranchPromotion", "StartDate");
            DropColumn("dbo.BranchPromotion", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BranchPromotion", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BranchPromotion", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ApplicationAction", "ActionUrl", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
