using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public record UpdateTagDTO
    {
        [StringLength(20, MinimumLength = 2)]        
        public string TagName { get; init; }
       
        [StringLength(10, MinimumLength = 2)] 
        public string TagColor { get; init; }
    }
}