using Syra.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.ViewModel
{
    public class BotDeploymentView
    {
        public Int64 Id { get; set; }

        public Int64 CustomerId { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }
        public string FacebookPage { get; set; }
        public string Website { get; set; }
        public string ContactPage { get; set; }
        public string ContactNo { get; set; }


        #region Bot Related Details
        public string WelcomeMessage { get; set; }

        public ICollection<MessageView> Messages { get; set; }
        public ICollection<BotQuestionAnswersView> BotQuestionAnswers { get; set; }
        public string BackGroundColor { get; set; }

        public string BotSecret { get; set; }
        public string BotURI { get; set; }

        public string WebSiteURI { get; set; }
        public string DomainName { get; set; }

        public string ChatBotGoal { get; set; }

        #endregion

        public DateTime DeploymentDate { get; set; }

        public string ResourceGroupName { get; set; }
        public string BlobStorageName { get; set; }
        public string WebSiteUrl { get; set; }
        public DateTime DeleteDate { get; set; }

        public Int64 LuisId { get; set; }
        public string LuisDomainName { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsPlanActive { get; set; }

        public string DeploymentScript { get; set; }
    }
}