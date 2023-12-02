using Unni.ToDo.Core.Models;

namespace Unni.ToDo.Core.Interfaces
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
