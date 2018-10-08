namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LuisResponses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Intent = c.String(),
                        Entity = c.String(),
                        Response = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        BotDomainId = c.Long(nullable: false),
                        BotDeploymentId = c.Long(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LuisDomains", t => t.BotDomainId, cascadeDelete: true)
                .ForeignKey("dbo.BotDeployments", t => t.BotDeploymentId, cascadeDelete: true)
                .Index(t => t.BotDomainId)
                .Index(t => t.BotDeploymentId);
            
            AddColumn("dbo.BotDeployments", "InheritStandardQA", c => c.Boolean(nullable: false));
            AddColumn("dbo.BotDeployments", "HasAdditionalResponse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LuisResponses", "BotDeploymentId", "dbo.BotDeployments");
            DropForeignKey("dbo.LuisResponses", "BotDomainId", "dbo.LuisDomains");
            DropIndex("dbo.LuisResponses", new[] { "BotDeploymentId" });
            DropIndex("dbo.LuisResponses", new[] { "BotDomainId" });
            DropColumn("dbo.BotDeployments", "HasAdditionalResponse");
            DropColumn("dbo.BotDeployments", "InheritStandardQA");
            DropTable("dbo.LuisResponses");
        }
    }
}
