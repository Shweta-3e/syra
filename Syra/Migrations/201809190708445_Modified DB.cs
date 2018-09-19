namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotDeployments", "FirstMessage", c => c.String());
            AddColumn("dbo.BotDeployments", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.BotDeployments", "EmbeddedScript", c => c.String());
            AddColumn("dbo.BotDeployments", "T_BotClientId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotDeployments", "T_BotClientId");
            DropColumn("dbo.BotDeployments", "EmbeddedScript");
            DropColumn("dbo.BotDeployments", "Status");
            DropColumn("dbo.BotDeployments", "FirstMessage");
        }
    }
}
