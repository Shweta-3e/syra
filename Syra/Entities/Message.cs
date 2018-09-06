using System;

namespace Syra.Admin.Entities
{
    public class Message
    {
        public Int64 CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public Int64 Id { get; set; }
        public string Text { get; set; }
        public int OrderBy { get; set; }
    }
}