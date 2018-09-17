namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BotDeployments", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BotDeployments", "Status", c => c.Int(nullable: false));
        }
    }
}
