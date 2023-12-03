using Unni.Admin.Application.DTOs;

namespace Unni.Admin.Application.Interfaces
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
