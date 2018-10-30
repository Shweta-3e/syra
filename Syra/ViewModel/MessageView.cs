using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.ViewModel
{
    public class MessageView
    {
        public Int64 CustomerId { get; set; }

        public Int64 Id { get; set; }
        public string Text { get; set; }
        public int OrderBy { get; set; }
    }
}