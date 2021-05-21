using System.ComponentModel.DataAnnotations;

namespace Todo.Api.DTOs
{
    public record CreateUserDTO
    {
        public string Username { get; init; }

        public string Email { get; init; }

        public string Password { get; init; }
    }
}