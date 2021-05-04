using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public interface UpdateToDoDTO
    {
        //   public Guid Id { get; init; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Description { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset Time { get; init; } 
        
        [StringLength(255, MinimumLength = 2)]
        public string Location { get; init; }

        [Required]
        public bool Done { get; init; }

        [Required]
        // [StringLength(255, MinimumLength = 2)]
        public int Priority { get; init; }
        public Guid FkTagId { get; init; }
        // public Guid FkUserId { get; init; }
    }
}