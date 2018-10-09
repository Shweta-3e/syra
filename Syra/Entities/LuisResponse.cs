using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class LuisResponse
    {
        public Int64 Id { get; set; }

        [Index("IX_IE_DOMAIN_DEPLOY_UX", 0, IsUnique = true)]
        public Int64 IEKey { get; set; }

        public string Intent { get; set; }
        public string Entity { get; set; }
        public string Response { get; set; }

        public bool IsActive { get; set; }

        public virtual LuisDomain BotDomain { get; set; }
        [Index("IX_IE_DOMAIN_DEPLOY_UX", 1, IsUnique = true)]
        public Int64 BotDomainId { get; set; }

        public virtual BotDeployment BotDeployment { get; set; }
        [Index("IX_IE_DOMAIN_DEPLOY_UX", 2, IsUnique = true)]
        public Int64? BotDeploymentId { get; set; }

    }
}