using System;

namespace TodoApi.DTOs
{
    public record UserDTO
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}