using System.ComponentModel.DataAnnotations;

namespace Todo.Api.DTOs
{
    public record LoginUserDTO
    {
        public string Login { get; init; }

        public string Password { get; init; }
    }
}