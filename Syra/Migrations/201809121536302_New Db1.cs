namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDb1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotDeployments", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotDeployments", "Status");
        }
    }
}
