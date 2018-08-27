using System;

namespace Syra.Admin.Entities
{
    public class BotDeployment
    {
        public Int64 Id { get; set; }

        public virtual Customer Customer { get; set; }
        public Int64 CustomerId { get; set; }

        public DateTime DeploymentDate { get; set; }

        public string ResourceGroupName { get; set; }
        public string BlobStorageName { get; set; }
        public string WebSiteUrl { get; set; }
        public DateTime DeleteDate { get; set; }

        public virtual LuisDomain LuisDomain { get; set; }
        public Int64 LuisId { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsPlanActive { get; set; }

        public string DeploymentScript { get; set; }
    }
}