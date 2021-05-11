using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public record UpdateUserDTO
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }

        [StringLength(40, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$", ErrorMessage = "The password should contain at least one letter and one number!")]
        public string NewPassword { get; init; }

        [Required]
        [StringLength(40, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$", ErrorMessage = "The password should contain at least one letter and one number!")]
        public string CurrentPassword { get; init; }
    }
}