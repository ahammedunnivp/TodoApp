using System.ComponentModel.DataAnnotations;
using Unni.ToDo.API.Enums;

namespace Unni.ToDo.API.DTOs
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
