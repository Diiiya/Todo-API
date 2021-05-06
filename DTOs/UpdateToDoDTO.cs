using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public record UpdateToDoDTO
    {
        [Required]
        [StringLength(255)]
        public string Description { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset Time { get; init; } 
        
        [StringLength(255)]
        public string Location { get; init; }
        public bool Done { get; init; }
        public int Priority { get; init; }
    }
}