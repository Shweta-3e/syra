namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocalDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotDeployments", "SecondMessage", c => c.String());
            AddColumn("dbo.BotDeployments", "DomainKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotDeployments", "DomainKey");
            DropColumn("dbo.BotDeployments", "SecondMessage");
        }
    }
}
