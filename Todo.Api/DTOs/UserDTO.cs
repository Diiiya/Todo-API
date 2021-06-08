using System;

namespace Todo.Api.DTOs
{
    public record UserDTO
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
    }
}