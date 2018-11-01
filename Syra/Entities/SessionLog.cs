using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class SessionLog
    {
        public SessionLog()
        {
            IsWrongAnswer=false;
        }

        public string SessionId { get; set; }
        public string ClickedLink { get; set; }
        public string IPAddress { get; set; }
        public string Region { get; set; }
        public string UserQuery { get; set; }
        public string BotAnswers { get; set; }
        public bool IsWrongAnswer { get; set; }
        public string LogDate { get; set; }
        public string Country { get; set; }
        public DateTime Log_Date { get; set; }
        public string LogTime { get; set; }
    }
}