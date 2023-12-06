using Microsoft.EntityFrameworkCore;
using Unni.Admin.Domain.Entities;
using Unni.Admin.Infrastructure.Data.Seeding;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Unni.Admin.Infrastructure.Context
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            var dataSeeder = new AdminDataSeeder(this);
            dataSeeder.SeedData();
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
