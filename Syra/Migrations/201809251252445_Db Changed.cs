namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotDeployments", "BlobConnectionString", c => c.String());
            AddColumn("dbo.BotDeployments", "ContainerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotDeployments", "ContainerName");
            DropColumn("dbo.BotDeployments", "BlobConnectionString");
        }
    }
}
