using System;
using System.Collections.Generic;

#nullable disable

namespace Todo.Api
{
    public partial class Tag
    {
        public Tag()
        {
            ToDos = new HashSet<ToDo>();
        }

        public Guid Id { get; set; }
        public string TagName { get; set; }
        public string TagColor { get; set; }
        public Guid FkUserId { get; set; }

        public virtual User FkUser { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
