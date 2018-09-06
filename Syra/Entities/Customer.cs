using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class Customer
    {
        public Customer()
        {
            CustomerPlans = new List<CustomerPlan>();
            BotDeployments = new List<BotDeployment>();
        }
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
     

        public ICollection<CustomerPlan> CustomerPlans { get; set; }

        public ICollection<BotDeployment> BotDeployments { get; set; }
    }
}