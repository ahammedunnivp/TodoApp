using System.ComponentModel.DataAnnotations;

namespace Unni.Admin.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }
}
