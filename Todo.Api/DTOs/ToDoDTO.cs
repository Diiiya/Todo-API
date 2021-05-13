using System;

namespace Todo.Api.DTOs
{
    public record ToDoDTO
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset Time { get; init; } 
        public string Location { get; init; }
        public bool Done { get; init; }
        public int Priority { get; init; } 
        public Guid FkTagId { get; init; }
        public Guid FkUserId { get; init; }

    }
}