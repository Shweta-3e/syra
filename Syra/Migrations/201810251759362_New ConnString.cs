namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewConnString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LuisResponses", "IX_IE_DOMAIN_DEPLOY_UX");
            AddColumn("dbo.LuisResponses", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.LuisResponses", "BotDomainId");
            CreateIndex("dbo.LuisResponses", "BotDeploymentId");
            DropColumn("dbo.LuisResponses", "IEKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LuisResponses", "IEKey", c => c.Long(nullable: false));
            DropIndex("dbo.LuisResponses", new[] { "BotDeploymentId" });
            DropIndex("dbo.LuisResponses", new[] { "BotDomainId" });
            DropColumn("dbo.LuisResponses", "Discriminator");
            CreateIndex("dbo.LuisResponses", new[] { "IEKey", "BotDomainId", "BotDeploymentId" }, unique: true, name: "IX_IE_DOMAIN_DEPLOY_UX");
        }
    }
}
