using System.ComponentModel.DataAnnotations;

namespace Todo.Api.DTOs
{
    public record LoginUserDTO
    {
        [Required]
        public string Login { get; init; }

        [Required]
        public string Password { get; init; }
    }
}