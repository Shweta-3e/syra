using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Syra.Admin.Entities
{
    public class GoalConversion
    {
        public Int64 Id { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual BotDeployment BotDeployment { get; set; }
        public Int64 BotDeploymentId { get; set; }

        public string LinkName { get; set; }
        public string LinkUrl { get; set; }
    }
}