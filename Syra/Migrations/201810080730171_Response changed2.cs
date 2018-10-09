namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Responsechanged2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LuisResponses", "IX_IE_DOMAIN_DEPLOY_UX");
            DropIndex("dbo.LuisResponses", "IX_I_EN_DOMAIN_DEPLOY_UX");
            CreateIndex("dbo.LuisResponses", new[] { "IEKey", "BotDomainId", "BotDeploymentId" }, unique: true, name: "IX_IE_DOMAIN_DEPLOY_UX");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LuisResponses", "IX_IE_DOMAIN_DEPLOY_UX");
            CreateIndex("dbo.LuisResponses", new[] { "BotDomainId", "BotDeploymentId" }, unique: true, name: "IX_I_EN_DOMAIN_DEPLOY_UX");
            CreateIndex("dbo.LuisResponses", "IEKey", unique: true, name: "IX_IE_DOMAIN_DEPLOY_UX");
        }
    }
}
