using System;
using System.Collections.Generic;

namespace Syra.Admin.Entities
{
    public class BotDeployment
    {
        public Int64 Id { get; set; }

        public virtual Customer Customer { get; set; }
        public Int64 CustomerId { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }
        public string FacebookPage { get; set; }
        public string Website { get; set; }
        public string ContactPage { get; set; }
        public string ContactNo { get; set; }


        #region Bot Related Details
        public string WelcomeMessage { get; set; }
        public string FirstMessage { get; set; }
        public string SecondMessage { get; set; }

        public ICollection<Message> Messages { get; set; }
        public ICollection<BotQuestionAnswers> BotQuestionAnswers { get; set; }
        public string BackGroundColor { get; set; }

        public string BotSecret { get; set; }
        public string BotURI { get; set; }

        public string WebSiteURI { get; set; }
        public string DomainName { get; set; }

        public string ChatBotGoal { get; set; }

        #endregion
        public Status Status { get; set; }
        public DateTime DeploymentDate { get; set; }

        public string ResourceGroupName { get; set; }
        public string BlobStorageName { get; set; }
        public string BlobConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string WebSiteUrl { get; set; }
        public DateTime DeleteDate { get; set; }

        public virtual LuisDomain LuisDomain { get; set; }
        public Int64 LuisId { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsPlanActive { get; set; }

        public string DeploymentScript { get; set; }
        public string EmbeddedScript { get; set; }
        public string T_BotClientId { get; set; }
        public string DomainKey { get; set; }
    }

    public enum Status
    {
        InProgress,
        Successful,
        Failed
    }

}