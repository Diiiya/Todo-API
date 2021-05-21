using System;
using Todo.Api.Models;

namespace Todo.Api.DTOs
{
    public record ToDoDTO
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public DateTimeOffset DateTime { get; init; }
        public string Location { get; init; }
        public bool Done { get; init; }
        public int Priority { get; init; }
        public Guid? FkTagId { get; init; }
        public Guid FkUserId { get; init; }

        //To return also Tag info
        public virtual TagDTO Tag {get; set;}

    }
}