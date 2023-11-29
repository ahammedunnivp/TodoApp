using Microsoft.EntityFrameworkCore;
using Unni.ToDo.API.Data.Models;

namespace Unni.ToDo.API.Data.Repositories
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(e => e.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}
