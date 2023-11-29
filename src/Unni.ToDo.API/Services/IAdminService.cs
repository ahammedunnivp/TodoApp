using Unni.ToDo.API.DTOs;
using Unni.ToDo.API.Enums;

namespace Unni.ToDo.API.Services
{
    public interface IAdminService
    {
        public CategoryDto GetCategoryById(int id);
        public IEnumerable<CategoryDto> GetCategories();
        public CategoryDto AddCategory(AddCategoryRequest addCategoryDto);

        public CategoryDto UpdateCategory(CategoryDto category);
        public void DeleteCategory(int id);
    }
}
