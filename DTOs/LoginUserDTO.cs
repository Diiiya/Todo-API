using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public record LoginUserDTO
    {
        [Required]
        public string Login { get; init; }

        [Required]
        public string Password { get; init; }
    }
}