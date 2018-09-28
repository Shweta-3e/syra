using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MikeHabibChatBot.Controllers
{
    static class Demo
    {
        public static string region;
        public static string ip;
        public static string uuid;
        public static string response;
    }

    //[EnableCors(origins: "http://whichbigdata.com", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs

    [Serializable]
    public class DemoController : ApiController
    {
        [HttpPost]
        public void SendMail(string Name, string IPAddress, string UniqueId, string Response)
        {
            Demo.region = Name;
            Demo.ip = IPAddress;
            Demo.uuid = UniqueId;
            Demo.response = Response;
        }
    }
}