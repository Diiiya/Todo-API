using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public record CreateUserDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^(?:.*[A-Za-z0-9])$", ErrorMessage = "You can only use letters and numbers!")]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }

        [Required]
        [StringLength(40, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$", ErrorMessage = "The password should contain at least one letter and one number!")]
        public string Password { get; init; }
    }
}