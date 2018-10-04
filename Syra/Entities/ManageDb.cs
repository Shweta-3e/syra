using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class ManageDb
    {
        public Int64 Id  { get; set; }
        public string  Intent { get; set; }
        public string Entity { get; set; }
        public string Response { get; set; }
    }
}