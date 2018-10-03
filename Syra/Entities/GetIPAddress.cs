using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class GetIPAddress
    {
        //public string ip { get; set; }
        //public string city { get; set; }
        //public string region { get; set; }
        //public string region_code { get; set; }
        //public string country { get; set; }
        //public string country_name { get; set; }
        //public string continent_code { get; set; }
        //public string in_eu { get; set; }
        //public string postal { get; set; }
        //public string latitude { get; set; }
        //public string longitude { get; set; }
        //public string timezone { get; set; }
        //public string utc_offset { get; set; }
        //public string country_calling_code { get; set; }
        //public string currency { get; set; }
        //public string languages { get; set; }
        //public string asn { get; set; }
        //public string org { get; set; }
        public string businessName { get; set; }
        public string businessWebsite { get; set; }
        public string city { get; set; }
        public string continent { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string ipName { get; set; }
        public string ipType { get; set; }
        public string isp { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string org { get; set; }
        public string query { get; set; }
        public string region { get; set; }
        public string status { get; set; }
    }
}