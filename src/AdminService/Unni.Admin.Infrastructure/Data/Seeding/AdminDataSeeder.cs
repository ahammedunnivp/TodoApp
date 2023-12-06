using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unni.Admin.Domain.Entities;
using Unni.Admin.Infrastructure.Context;

namespace Unni.Admin.Infrastructure.Data.Seeding
{
    public class AdminDataSeeder
    {
        private readonly AdminDbContext _dbContext;

        public AdminDataSeeder(AdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            if(!_dbContext.Categories.Any())
            {
                var initialCategories = new List<CategoryEntity>
                {
                    new CategoryEntity{ Name = "Work", Description = "Official Purpose"},
                    new CategoryEntity{ Name = "Personal", Description = "Personal Purpose"}
                };

                _dbContext.Categories.AddRange(initialCategories);
                _dbContext.SaveChanges();
            }
        }
    }
}
