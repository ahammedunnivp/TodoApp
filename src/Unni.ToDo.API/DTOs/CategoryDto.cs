using System.ComponentModel.DataAnnotations;

namespace Unni.ToDo.API.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
