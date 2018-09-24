namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BotDeployments", "DomainKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BotDeployments", "DomainKey", c => c.String());
        }
    }
}
