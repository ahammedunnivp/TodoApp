using Unni.ToDo.Common.Models;

namespace Unni.ToDo.Common.Interfaces
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
