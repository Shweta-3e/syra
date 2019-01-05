namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoalConversions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BotDeploymentId = c.Long(nullable: false),
                        LinkName = c.String(),
                        LinkUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BotDeployments", t => t.BotDeploymentId, cascadeDelete: true)
                .Index(t => t.BotDeploymentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoalConversions", "BotDeploymentId", "dbo.BotDeployments");
            DropIndex("dbo.GoalConversions", new[] { "BotDeploymentId" });
            DropTable("dbo.GoalConversions");
        }
    }
}
