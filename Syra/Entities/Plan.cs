using System;
using System.Collections.Generic;

namespace Syra.Admin.Entities
{
    public class Plan
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 Days { get; set; }
        public Int64 Cost { get; set; }
        public bool IsLogAvailable { get; set; }
        public bool IsAnalyticsAvailable { get; set; }
        public int LogRetentionDays { get; set; }

        public ICollection<CustomerPlan> CustomerPlans { get; set; }
    }
}