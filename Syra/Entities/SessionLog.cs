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
            BotResponse = "Responded Correctly";
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
        public string BotResponse { get; set; }
        public DateTime Log_Date { get; set; }
        public string LogTime { get; set; }

        static public class State
        {
            public static string region;
            public static string ip;
            public static string uuid;
        }
        public class Demo
        {
            public static string region;
            public static string ip;
            public static string uuid;
            public static string response;
        }

    }
}