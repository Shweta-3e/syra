using System;
using System.Collections.Generic;
using Syra.Admin.ViewModel;

namespace Syra.Admin.Entities
{
    public class CustomerPlan
    {
        public Int64 Id { get; set; }

        public virtual Customer Customer { get; set; }
        public Int64 CustomerId { get; set; }

        public virtual Plan Plan { get; set; }
        public Int64 PlanId { get; set; }

        public DateTime ActivationDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        //public ICollection<CustomerPlanView> CustomerPlanViews { get; set; }
    }
}