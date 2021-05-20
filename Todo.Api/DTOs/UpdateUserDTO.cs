using System.ComponentModel.DataAnnotations;

namespace Todo.Api.DTOs
{
    public record UpdateUserDTO
    {
        public string Email { get; init; }

        public string NewPassword { get; init; }

        public string CurrentPassword { get; init; }
    }
}