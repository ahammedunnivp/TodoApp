using Unni.Admin.Domain.Entities;

namespace Unni.Admin.Domain.Interfaces
{
    public interface IAdminRepository
    {
        bool CheckCategoryIdExists(int categoryId);
        CategoryEntity? GetCategoryById(int categoryId);
        IEnumerable<CategoryEntity> GetAllCategories();
        CategoryEntity AddCategory(CategoryEntity item);
        void UpdateCategory(CategoryEntity item);
        void DeleteCategory(int id);
    }
}
