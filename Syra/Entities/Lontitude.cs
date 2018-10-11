﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class Lontitude
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
        public Int64 epochtime { get; set; }
        public Int64 counttime { get; set; }
    }
    public class LowHighTime
    {
        public Int64 epochtime { get; set; }
        public DateTime date { get; set; }
        public Int64 timecount { get; set; }
        public string status { get; set; }
    }

    
}