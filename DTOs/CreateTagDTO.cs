using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public record CreateTagDTO
    {

        [Required]
        [StringLength(20, MinimumLength = 2)]        
        public string TagName { get; init; }
        
        [Required]
        [StringLength(10, MinimumLength = 2)] 
        public string TagColor { get; init; }
    }
}