using System;

namespace Todo.Api.Models
{
    public record ToDo
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public DateTimeOffset DateTime { get; init; }
        public string Location { get; init; }
        public bool Done { get; init; }
        public int Priority { get; init; } 
        public Guid? FkTagId { get; init; }
        public Guid FkUserId { get; init; }

        // in order to use foreign keys and relations
        public virtual Tag Tag {get; set;}
        public User User {get; init;}
    }
}