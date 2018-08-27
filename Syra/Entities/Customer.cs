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

        public string CompanyName { get; set; }
        public string FacebookPage { get; set; }
        public string Website { get; set; }
        public string ContactPage { get; set; }

        #region Bot Related Details
        public string WelcomeMessage { get; set; }

        public ICollection<Message> Messages { get; set; }

        public string Color { get; set; }

        public string BotSecret { get; set; }
        public string BotURI { get; set; }

        public string WebSiteURI { get; set; }
        public string DomainName { get; set; }

        #endregion

        public ICollection<CustomerPlan> CustomerPlans { get; set; }

        public ICollection<BotDeployment> BotDeployments { get; set; }
    }
}