using System;
using System.Collections.Generic;

namespace Todo.Api.Models
{
    public record Tag
    {
        public Guid Id { get; init; }
        public string TagName { get; init; }
        public string TagColor { get; init; }
        public Guid FkUserId { get; init; }

        // in order to use foreign keys and relations
        public List<ToDo> ToDos {get; init;}
        public User User {get; init;}
    }
}