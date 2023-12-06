using Microsoft.EntityFrameworkCore;
using Unni.Admin.Infrastructure.Data.Seeding;
using Unni.Todo.Domain.Entities;

namespace Unni.Todo.Infrastructure.Context
{
    public class ToDoDBContext : DbContext
    {
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options) : base(options)
        {
            Database.EnsureCreated();
            SeedInitialData();
        }

        public DbSet<TodoItemEntity> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItemEntity>()
                .HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
        }

        private void SeedInitialData()
        {
            var dataSeeder = new TodoDataSeeder(this);
            dataSeeder.SeedData();
        }
    }
}
