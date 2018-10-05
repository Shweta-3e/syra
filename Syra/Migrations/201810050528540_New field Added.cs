namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewfieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManageDbs", "BotDomain", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManageDbs", "BotDomain");
        }
    }
}
