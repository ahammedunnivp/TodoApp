using System.ComponentModel.DataAnnotations;

namespace Unni.ToDo.API.DTOs
{
    public class AddCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
