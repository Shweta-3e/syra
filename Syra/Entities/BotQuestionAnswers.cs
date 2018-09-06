using System;

namespace Syra.Admin.Entities
{
    public class BotQuestionAnswers
    {
        public Int64 Id { get; set; }

        public  virtual BotDeployment BotDeployment{ get; set; }
        public Int64 BotDeploymentId { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
    }

}