using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class LuisResponse
    {
        public Int64 Id { get; set; }
        public string Intent { get; set; }
        public string Entity { get; set; }
        public string Response { get; set; }

        public bool IsActive { get; set; }

        public virtual LuisDomain BotDomain { get; set; }
        public Int64 BotDomainId { get; set; }
    }

    public class LuisResponseForCustomer : LuisResponse
    {
        public virtual BotDeployment BotDeployment { get; set; }
        public Int64 BotDeploymentId { get; set; }
    }
}