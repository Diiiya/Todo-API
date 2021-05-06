using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public record User
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
        public bool Deleted { get; init; }
        
        // in order to use foreign keys and relations
        public List<ToDo> ToDos {get; init;}
    }
}