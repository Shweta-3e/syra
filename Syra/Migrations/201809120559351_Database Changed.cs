namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseChanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BotDeployments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        Name = c.String(),
                        CompanyName = c.String(),
                        FacebookPage = c.String(),
                        Website = c.String(),
                        ContactPage = c.String(),
                        ContactNo = c.String(),
                        WelcomeMessage = c.String(),
                        BackGroundColor = c.String(),
                        BotSecret = c.String(),
                        BotURI = c.String(),
                        WebSiteURI = c.String(),
                        DomainName = c.String(),
                        ChatBotGoal = c.String(),
                        DeploymentDate = c.DateTime(nullable: false),
                        ResourceGroupName = c.String(),
                        BlobStorageName = c.String(),
                        WebSiteUrl = c.String(),
                        DeleteDate = c.DateTime(nullable: false),
                        LuisId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsPlanActive = c.Boolean(nullable: false),
                        DeploymentScript = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.LuisDomains", t => t.LuisId)
                .Index(t => t.CustomerId)
                .Index(t => t.LuisId);
            
            CreateTable(
                "dbo.BotQuestionAnswers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BotDeploymentId = c.Long(nullable: false),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BotDeployments", t => t.BotDeploymentId, cascadeDelete: true)
                .Index(t => t.BotDeploymentId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        UserId = c.String(),
                        RegisterDate = c.DateTime(nullable: false),
                        ContactNo = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Address3 = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        JobTitle = c.String(),
                        PricingPlan = c.String(),
                        BusinessRequirement = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerPlans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        PlanId = c.Long(nullable: false),
                        ActivationDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.PlanId)
                .Index(t => t.CustomerId)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Types = c.String(),
                        MonthlyCharge = c.String(),
                        SetupFees = c.String(),
                        Contract = c.String(),
                        SiteSpecification = c.String(),
                        KnowledgeDomain = c.String(),
                        AllowedBotLimit = c.Int(nullable: false),
                        InitialTraining = c.String(),
                        AdvanceTraining = c.String(),
                        TextQuery = c.String(),
                        PagesScrapping = c.String(),
                        Entities = c.String(),
                        Intent = c.String(),
                        LogRetainingDay = c.String(),
                        Analyticsplan = c.String(),
                        EmbedWidget = c.String(),
                        FBWidget = c.String(),
                        SlackWidget = c.String(),
                        SkypeWidget = c.String(),
                        TelegramWidget = c.String(),
                        KikWidget = c.String(),
                        SupportAvailability = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LuisDomains",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Details = c.String(),
                        Category = c.String(),
                        LuisAppId = c.String(),
                        LuisAppKey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        Text = c.String(),
                        OrderBy = c.Int(nullable: false),
                        BotDeployment_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.BotDeployments", t => t.BotDeployment_Id)
                .Index(t => t.CustomerId)
                .Index(t => t.BotDeployment_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Messages", "BotDeployment_Id", "dbo.BotDeployments");
            DropForeignKey("dbo.Messages", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.BotDeployments", "LuisId", "dbo.LuisDomains");
            DropForeignKey("dbo.BotDeployments", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerPlans", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.CustomerPlans", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.BotQuestionAnswers", "BotDeploymentId", "dbo.BotDeployments");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Messages", new[] { "BotDeployment_Id" });
            DropIndex("dbo.Messages", new[] { "CustomerId" });
            DropIndex("dbo.CustomerPlans", new[] { "PlanId" });
            DropIndex("dbo.CustomerPlans", new[] { "CustomerId" });
            DropIndex("dbo.BotQuestionAnswers", new[] { "BotDeploymentId" });
            DropIndex("dbo.BotDeployments", new[] { "LuisId" });
            DropIndex("dbo.BotDeployments", new[] { "CustomerId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Messages");
            DropTable("dbo.LuisDomains");
            DropTable("dbo.Plans");
            DropTable("dbo.CustomerPlans");
            DropTable("dbo.Customers");
            DropTable("dbo.BotQuestionAnswers");
            DropTable("dbo.BotDeployments");
        }
    }
}
