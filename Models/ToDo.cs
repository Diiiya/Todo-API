using System;

namespace TodoApi.Models
{
    public record ToDo
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset Time { get; init; } // is it necessary?
        public string Location { get; init; }
        public bool Done { get; init; }
        public int Priority { get; init; } //should i do it with enum?
        public Guid FkUserId { get; init; }



        public Guid FkTagId { get; init; }
        public Tag TagItem { get; init; }
    }
}