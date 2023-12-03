using Microsoft.EntityFrameworkCore;
using Unni.ToDo.Core.Models;

namespace Unni.ToDo.Infrastructure.Data.Repositories
{
    public class ToDoDBContext : DbContext
    {
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options) : base(options)
        {
        }

        public DbSet<TodoItemEntity> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItemEntity>()
                .HasKey(t => t.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
