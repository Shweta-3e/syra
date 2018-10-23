using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Helper
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public bool RedirectToLogin { get; set; }
        public string GetResponse()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}