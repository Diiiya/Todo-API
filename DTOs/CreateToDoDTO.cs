using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs
{
    public record CreateToDoDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Description { get; init; }
        public DateTimeOffset Date { get; init; }
        public DateTimeOffset Time { get; init; } // is it necessary?

        [StringLength(255, MinimumLength = 3)]
        public string Location { get; init; }
        // public bool Done { get; init; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public int Priority { get; init; } //should i do it with enum?
        // needs to be required
        public Guid FkTagId { get; init; }

        // needs to be required
        public Guid FkUserId { get; init; }
    }
}