using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Syra.Admin.Entities
{
    public class Plan
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        
        public string Types { get; set; }
        
        public string MonthlyCharge { get; set; }
        
        public string SetupFees { get; set; }
        
        public string Contract { get; set; }

        public string SiteSpecification { get; set; }

        public string KnowledgeDomain { get; set; }

        public int AllowedBotLimit { get; set; }

        public string InitialTraining { get; set; }

        public  string AdvanceTraining { get; set; }

        public string TextQuery { get; set; }

        public string PagesScrapping { get; set; }

        public string Entities { get; set; }

        public string Intent { get; set; }

        public string LogRetainingDay { get; set; }

        public string Analyticsplan { get; set; }

        public string EmbedWidget { get; set; }

        public string FBWidget { get; set; }

        public string SlackWidget { get; set; }

        public string SkypeWidget { get; set; }
        public string TelegramWidget { get; set; }
        public string KikWidget { get; set; }
        public string SupportAvailability { get; set; }

        public ICollection<CustomerPlan> CustomerPlans { get; set; }
    }
}