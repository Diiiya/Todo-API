using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public record LoginUserDTO
    {
        [Required]
        public string Login { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$", ErrorMessage = "The password should contain at least one letter and one number!")]
        public string Password { get; init; }
    }
}