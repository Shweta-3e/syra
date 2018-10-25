using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Syra.Admin.Entities;

namespace Syra.Admin.ViewModel
{
    public class CustomerPlanView
    {
        public Int64 Id { get; set; }

        public Int64 CustomerId { get; set; }

        public Int64 PlanId { get; set; }

        public string PlanName { get; set; }

        public int AllowedBotLimit { get; set; }

        public DateTime ActivationDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        
    }
}