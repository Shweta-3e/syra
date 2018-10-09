namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Responsechanged : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LuisResponses", new[] { "BotDomainId" });
            DropIndex("dbo.LuisResponses", new[] { "BotDeploymentId" });
            AddColumn("dbo.LuisResponses", "IEKey", c => c.Long(nullable: false));
            CreateIndex("dbo.LuisResponses", "IEKey", unique: true, name: "IX_IE_DOMAIN_DEPLOY_UX");
            CreateIndex("dbo.LuisResponses", new[] { "BotDomainId", "BotDeploymentId" }, unique: true, name: "IX_I_EN_DOMAIN_DEPLOY_UX");
            DropColumn("dbo.LuisResponses", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LuisResponses", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropIndex("dbo.LuisResponses", "IX_I_EN_DOMAIN_DEPLOY_UX");
            DropIndex("dbo.LuisResponses", "IX_IE_DOMAIN_DEPLOY_UX");
            DropColumn("dbo.LuisResponses", "IEKey");
            CreateIndex("dbo.LuisResponses", "BotDeploymentId");
            CreateIndex("dbo.LuisResponses", "BotDomainId");
        }
    }
}
