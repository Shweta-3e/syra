using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Helper
{
    public class PaginatedResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public long Count { get; set; }
        public IEnumerable<object> Entities { get; set; }
        public int TotalPages { get; set; }
        public string GetResponse()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}