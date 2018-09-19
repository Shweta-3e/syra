using Syra.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.ViewModel
{
    public class CustomerView
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string UserId { get; set; }

        public DateTime RegisterDate { get; set; }
        public string ContactNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }


        public string JobTitle { get; set; }
        public string PricingPlan { get; set; }
        public string BusinessRequirement { get; set; }
       
        public string T_ClientId { get; set; }


        public ICollection<CustomerPlanView> CustomerPlans { get; set; }

        public ICollection<BotDeploymentView> BotDeployments { get; set; }
    }
}