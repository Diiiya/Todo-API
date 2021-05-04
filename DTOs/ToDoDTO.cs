using System;

namespace TodoApi.DTOs
{
    public record ToDoDTO
    {
        //what exactly should be missing?
        public Guid Id { get; init; }
        public string Description { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset Time { get; init; } // is it necessary?
        public string Location { get; init; }
        public bool Done { get; init; }
        public int Priority { get; init; } //should i do it with enum?
        public Guid FkTagId { get; init; }
        public Guid FkUserId { get; init; }
    }
}