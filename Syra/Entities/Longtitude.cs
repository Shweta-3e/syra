﻿using Newtonsoft.Json;
using System;
using System.Collections;
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
        //public string name { get; set; }
        public float value { get; set; }
        public string code { get; set; }
        public float z { get; set; }
        //public string code3 { get; set; }
        public string drilldown { get; set; }
    }
    public class LowHighTime
    {
        public string drilldown { get; set; }
        public Int64 epochtime { get; set; }
        public string status { get; set; }
    }
    public class LowHighTimedate {
        public string EpochDate { get; set; }
        public Int64 hour { get; set; }
        public string status { get; set; }
    }
    public class TimeDateJson {
        public string drilldown { get; set; }
        public Int64 epochtime { get; set; }
        public Int64 count { get; set; }
    }

    //public class CountHour
    //{
    //    public Int64 hour { get; set; }
    //    public Int64 hourcount { get; set; }
    //}
    public class Logtime
    {
        public DateTime time { get; set; }
    }
    public class USARegion
    {
        public string hckey { get; set; }
        public string name { get; set; }
    }
    public class USALocation
    {
        [JsonProperty(PropertyName = "hc-key")]
        public string hckey { get; set; }
        public float value { get; set; }
        public float z { get; set; }
        public string name { get; set; }
    }
   
}