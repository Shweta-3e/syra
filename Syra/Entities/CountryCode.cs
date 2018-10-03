using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class CountryCode
    {
        public string name { get; set; }
        public System.Collections.ObjectModel.Collection<topLevelDomain> topLevelDomain { get; set; }
        public string alpha2Code { get; set; }
        public string alpha3Code { get; set; }
    }
    public class topLevelDomain
    {
    }
}