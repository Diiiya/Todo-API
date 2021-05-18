using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.DTOs
{
    public record CreateToDoDTO
    {
        [Required]
        [StringLength(255)]
        public string Description { get; init; }
        public DateTimeOffset DateTime { get; init; }

        [StringLength(255)]
        public string Location { get; init; }
        
        [Range(0, 3)]
        public int Priority { get; init; }
        [Required]
        public Guid FkTagId { get; init; }

        [Required]
        public Guid FkUserId { get; init; }
    }
}