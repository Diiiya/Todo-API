using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.DTOs
{
    public record UpdateToDoDTO
    {
        [Required]
        [StringLength(255)]
        public string Description { get; init; }
        public DateTimeOffset DateTime { get; init; }
        
        [StringLength(255)]
        public string Location { get; init; }
        public int Priority { get; init; }
    }
}