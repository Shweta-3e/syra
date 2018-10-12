using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class GetIPAddress
    {
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
    public class GeoLocation
    {
        public string IP { get; set; }
        public string Country { get; set; }
        public string Countrycode { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Timezone { get; set; }
        public string ISP { get; set; }
        public string Organization { get; set; }
        public string AS { get; set; }
    }

}