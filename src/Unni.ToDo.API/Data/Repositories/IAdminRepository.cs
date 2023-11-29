using Unni.ToDo.API.Data.Models;
using Unni.ToDo.API.DTOs;

namespace Unni.ToDo.API.Data.Repositories
{
    public interface IAdminRepository
    {
        bool CheckCategoryIdExists(int categoryId);
        CategoryEntity GetCategoryById(int categoryId);
        IEnumerable<CategoryEntity> GetAllCategories();
        CategoryEntity AddCategory(CategoryEntity item);
        void UpdateCategory(CategoryEntity item);
        void DeleteCategory(int id);
    }
}
