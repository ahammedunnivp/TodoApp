using System.ComponentModel.DataAnnotations;

namespace Unni.ToDo.Core.DTOs
{
    public class CreateTodoRequest
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Difficulty { get; set; }
        public string? Category { get; set; }
    }
}
