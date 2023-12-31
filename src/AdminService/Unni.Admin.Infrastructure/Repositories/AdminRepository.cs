﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Unni.Admin.Domain.Entities;
using Unni.Admin.Domain.Interfaces;
using Unni.Admin.Infrastructure.Context;

namespace Unni.Admin.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AdminDbContext _dbContext;
        private readonly ILogger _logger;

        public AdminRepository(AdminDbContext dbContext, ILogger<IAdminRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public CategoryEntity AddCategory(CategoryEntity item)
        {
            _logger.LogInformation("A new category item adding to DB");
            _dbContext.Categories.Add(item);
            return item;
        }

        public bool CheckCategoryIdExists(int categoryId)
        {
            return _dbContext.Categories.Any(e => e.Id == categoryId);
        }

        public void DeleteCategory(int id)
        {
            var item = _dbContext.Categories.Find(id);
            if (item != null)
            {
                _logger.LogInformation("Deleting the category with Id: {id}", id);
                _dbContext.Categories.Remove(item);
            }
        }

        public IEnumerable<CategoryEntity> GetAllCategories()
        {
            _logger.LogInformation("Fething category items from DB");
            return _dbContext.Categories.AsQueryable().ToList();
        }

        public CategoryEntity? GetCategoryById(int categoryId)
        {
            _logger.LogInformation("Fething category item with Id: {id}", categoryId);

            var todoItem = _dbContext.Categories.Find(categoryId);
            return todoItem;
        }

        public void UpdateCategory(CategoryEntity item)
        {
            _logger.LogInformation("Updaing category item with Id: {id}", item.Id);
            _dbContext.Categories.Update(item);
        }
    }
}
