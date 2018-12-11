using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class BotContent
    {
        public int ID { get; set; }
        public string Intent { get; set; }
        public string Entity { get; set; }
        public string BotReply { get; set; }
    }
}