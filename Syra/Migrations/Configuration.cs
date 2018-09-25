namespace Syra.Admin.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Syra.Admin.DbContexts;
    using Syra.Admin.Entities;
    using Syra.Admin.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Syra.Admin.DbContexts.SyraDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Syra.Admin.DbContexts.SyraDbContext context)
        {
            SyraDbContext db = new SyraDbContext();
            if (!context.Plans.Any())
            {
                var plan = new List<Plan>
            {
                new Plan { Name = "Starter!", Types = "Startups", MonthlyCharge ="$0", SetupFees = "None", Contract = "No Minimums",SiteSpecification="None",KnowledgeDomain="None",InitialTraining="NA",AdvanceTraining="NA",TextQuery="Less than 100 per month",PagesScrapping="None",Entities="None",Intent="None",LogRetainingDay="None",Analyticsplan="None",EmbedWidget="1 site",FBWidget="1 site",SlackWidget="No",SkypeWidget="No",TelegramWidget="No",KikWidget="No",SupportAvailability="Support Site, Community"},
                new Plan { Name = "Get Going!", Types = "Small Businesses", MonthlyCharge = "$25", SetupFees = "None", Contract = "No Minimums",SiteSpecification="Basic",KnowledgeDomain="None",InitialTraining="NA",AdvanceTraining="NA",TextQuery="Less than 100 per month",PagesScrapping="None",Entities="NA",Intent="NA",LogRetainingDay="7 days historical",Analyticsplan="Basic",EmbedWidget="1 site",FBWidget="1 site",SlackWidget="No",SkypeWidget="No",TelegramWidget="No",KikWidget="No",SupportAvailability="Support Site, Community, Email response in max of 48 hours" },
                new Plan { Name = "Almost There!", Types = "Medium Businesses", MonthlyCharge = "$50", SetupFees = "Included", Contract = "6 Months Minimum",SiteSpecification="Basic",KnowledgeDomain="Any one",InitialTraining="1 time, soon after initial setup",AdvanceTraining="1 time, in the first month of installation",TextQuery="Less than 100 per month",PagesScrapping="100",Entities="Unlimited",Intent="Unlimited",LogRetainingDay="30 days Historical",Analyticsplan="Basic",EmbedWidget="Unlimited sites",FBWidget="Unlimited Sites",SlackWidget="Any one",SkypeWidget="Any one",TelegramWidget="Any one",KikWidget="Any one",SupportAvailability="Support Site, Community, Email response in max of 48 hours"},
                new Plan { Name = "There!", Types = "Enterprises", MonthlyCharge = "$100", SetupFees = "Included", Contract = "6 Months Minimum",SiteSpecification="Extensive",KnowledgeDomain="Max of 2",InitialTraining="As many times as required",AdvanceTraining="As many times as required",TextQuery="Less than 1000 per month",PagesScrapping="1000",Entities="Unlimited",Intent="Unlimited",LogRetainingDay="length of Contract",Analyticsplan="Advanced",EmbedWidget="Unlimited sites.",FBWidget="Unlimited pages.",SlackWidget="All",SkypeWidget="All",TelegramWidget="All",KikWidget="All",SupportAvailability="Support Site, Community, Email response in max of 4 hours" },
                new Plan { Name = "Custom There!", Types = "Enterprises", MonthlyCharge = "Dependes", SetupFees = "Dependes", Contract = "Dependes",SiteSpecification="Complete",KnowledgeDomain="As many as required",InitialTraining="As many times as required",AdvanceTraining="As many times as required",TextQuery="As many times as required",PagesScrapping="As many times as reuired",Entities="Unlimited",Intent="Unlimited",LogRetainingDay="Lifetime",Analyticsplan="Advanced",EmbedWidget="Unlimited sites",FBWidget="Unlimited Pages",SlackWidget="All",SkypeWidget="All",TelegramWidget="All",KikWidget="All",SupportAvailability="Phone. Access to Development Team. Response time of less than 2 hours" }
            };
                plan.ForEach(x => context.Plans.Add(x));
                context.SaveChanges();
            }

            if (!context.LuisDomains.Any())
            {
                var luisdomain = new List<LuisDomain>
            {
                new LuisDomain { Name = "TEST BOT", Details = "TEST BOT", Category ="Deploy Website", LuisAppId = "APP123", LuisAppKey = "Test123"},
            };
                luisdomain.ForEach(x => context.LuisDomains.Add(x));
                context.SaveChanges();
            }

            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            string[] roleNames = { "Customer", "Admin" };
            IdentityResult roleResult;
            foreach (var rolename in roleNames)
            {
                if (!rolemanager.RoleExists(rolename))
                {
                    roleResult = rolemanager.Create(new IdentityRole(rolename));
                }
            }


            var userStore = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user = new ApplicationUser
            {
                Email = "admin@thirdeyedata.io",
                UserName = "admin@thirdeyedata.io"
            };
            var result = manager.Create(user, "123456");
            if (result.Succeeded)
            {
                manager.AddToRole(user.Id, "Admin");
            }
            context.SaveChanges();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }
    }
}
