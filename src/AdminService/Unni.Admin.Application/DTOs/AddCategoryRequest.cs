using System.ComponentModel.DataAnnotations;

namespace Unni.Admin.Application.DTOs
{
    public class AddCategoryRequest
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
