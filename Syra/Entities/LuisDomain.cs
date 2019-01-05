using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Syra.Admin.Entities
{
    public class LuisDomain {

        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; } 
        public string Category { get; set; }
        public string LuisAppId { get; set; }
        public string LuisAppKey { get; set; }
        public ICollection<LuisResponse> LuisResponses { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<BotDeployment> BotDeployments { get; set; }
    }
}