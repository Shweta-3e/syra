using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.ViewModel
{
    public class BotQuestionAnswersView
    {
        public Int64 Id { get; set; }

        public Int64 BotDeploymentId { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
    }
}