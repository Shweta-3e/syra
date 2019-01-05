using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.ViewModel
{
    public class BotGoalConversionsView
    {
        public Int64 Id { get; set; }
        public Int64 BotDeploymentId { get; set; }
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }

    }
}