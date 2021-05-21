using System;

namespace Todo.Api.DTOs
{
    public record TagDTO
    {
        public Guid Id { get; init; }
        public string TagName { get; init; }
        public string TagColor { get; init; } 
        public Guid FkUserId { get; init; }

    }
}