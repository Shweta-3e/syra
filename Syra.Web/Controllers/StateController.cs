using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MikeHabibChatBot.Controllers
{
    static class State
    {
        public static string region;
        public static string ip;
        public static string uuid;
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
   
    [Serializable]
    public class StateController : ApiController
    {
        [HttpPost]
        public void SendMail(string Name,string IPAddress,string UniqueId)
        {
            State.region = Name;
            State.ip = IPAddress;
            State.uuid = UniqueId;
            Demo.response = null;
        }
    }   
}