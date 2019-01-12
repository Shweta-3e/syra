using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Entities
{
    public class BaseLogin
    {
        public Int64 ID { get; set; }
        public string UserID { get; set; }
        public string Token { get; set; }
        public DateTime LoginDate { get; set; }
    }
}