using System.ComponentModel.DataAnnotations;

namespace Unni.ToDo.Common.DTOs
{
    public class AddCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
