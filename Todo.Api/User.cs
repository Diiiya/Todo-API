using System;
using System.Collections.Generic;

#nullable disable

namespace Todo.Api
{
    public partial class User
    {
        public User()
        {
            Tags = new HashSet<Tag>();
            ToDos = new HashSet<ToDo>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
