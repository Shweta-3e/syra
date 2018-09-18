namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dbupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotDeployments", "SecondMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotDeployments", "SecondMessage");
        }
    }
}
