using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class Longtitude
    {
        public string Countries { get; set; }
        public string UserQuery { get; set; }
        public string Links { get; set; }
        public string UserId { get; set; }
        public string righans { get; set; }
        public string wrongans { get; set; }
        
    }
    public class Location
    {
        public string name { get; set; }
        public float value { get; set; }
        public string code { get; set; }
        public string code3 { get; set; }
    }
    public class LowHighTime
    {
        public Int64 epochtime { get; set; }
        public DateTime date { get; set; }
        public Int64 timecount { get; set; }
        public string status { get; set; }
    }
    public class USARegion
    {
        public string hckey { get; set; }
    }
    public class USALocation
    {
        [JsonProperty(PropertyName = "hc-key")]
        public string hckey { get; set; }
        public float value { get; set; }
        private static char dash = '-';
        private static string key = "key";
    }
   
}