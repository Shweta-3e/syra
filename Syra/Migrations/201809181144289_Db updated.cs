namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dbupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotDeployments", "FirstMessage", c => c.String());
            AddColumn("dbo.BotDeployments", "T_BotClientId", c => c.String());
            DropColumn("dbo.Customers", "T_ClientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "T_ClientId", c => c.String());
            DropColumn("dbo.BotDeployments", "T_BotClientId");
            DropColumn("dbo.BotDeployments", "FirstMessage");
        }
    }
}
