using System;

namespace TodoApi.DTOs
{
    public record TagDTO
    {
        public Guid Id { get; init; }
        public string TagName { get; init; }
        public string TagColor { get; init; } 

    }
}