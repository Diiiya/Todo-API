using System;
using System.Collections.Generic;

#nullable disable

namespace Todo.Api
{
    public partial class ToDo
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Location { get; set; }
        public bool Done { get; set; }
        public int Priority { get; set; }
        public Guid? FkTagId { get; set; }
        public Guid FkUserId { get; set; }

        public virtual Tag FkTag { get; set; }
        public virtual User FkUser { get; set; }
    }
}
