using Microsoft.AspNet.Identity.EntityFramework;
using Syra.Admin.Entities;
using Syra.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Syra.Admin.DbContexts
{
    
    public class SyraDbContext:  IdentityDbContext<ApplicationUser>, ISyraDbContext
    {
        public SyraDbContext() : base("SyraDbContext", throwIfV1Schema: false)
        {

        }
      
        public static SyraDbContext Create()
        {
            return new SyraDbContext();
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerPlan> CustomerPlans { get; set; }
        public DbSet<BotDeployment> BotDeployments { get; set; }
        public DbSet<LuisDomain> LuisDomains { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<LuisResponse> ManageDbs { get; set; }
        public DbSet<BotQuestionAnswers> BotQuestionAnswers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BotDeployment>()
                .HasRequired(c => c.Customer)
                .WithMany(c => c.BotDeployments)
                .HasForeignKey(c => c.CustomerId).
                WillCascadeOnDelete(true);

            modelBuilder.Entity<BotDeployment>()
                .HasRequired(c => c.LuisDomain)
                .WithMany(c => c.BotDeployments)
                .HasForeignKey(c => c.LuisId).
                WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerPlan>()
               .HasRequired(c => c.Plan)
               .WithMany(c => c.CustomerPlans)
               .HasForeignKey(c => c.PlanId).
               WillCascadeOnDelete(false);

            modelBuilder.Entity<LuisResponse>()
                .HasRequired(c => c.BotDomain)
                .WithMany(c => c.LuisResponses)
                .HasForeignKey(c => c.BotDomainId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<LuisResponseForCustomer>()
                .HasRequired(c => c.BotDeployment)
                .WithMany(c => c.LuisResponsesforcusts)
                .HasForeignKey(c => c.BotDeploymentId)
                .WillCascadeOnDelete(true);
        }

    }

    public interface ISyraDbContext
    {
     
      DbSet<Customer> Customer { get; set; }
      DbSet<CustomerPlan> CustomerPlans { get; set; }
      DbSet<BotDeployment> BotDeployments { get; set; }
      DbSet<LuisDomain> LuisDomains { get; set; }
      DbSet<Message> Messages { get; set; }
      DbSet<Plan> Plans { get; set; }
    }
}