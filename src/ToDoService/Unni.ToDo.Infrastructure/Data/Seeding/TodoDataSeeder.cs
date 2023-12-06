using Unni.Todo.Domain.Entities;
using Unni.Todo.Infrastructure.Context;

namespace Unni.Admin.Infrastructure.Data.Seeding
{
    public class TodoDataSeeder
    {
        private readonly ToDoDBContext _dbContext;

        public TodoDataSeeder(ToDoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            if (!_dbContext.ToDoItems.Any())
            {
                var initialTodos = new List<TodoItemEntity>
                {
                    new TodoItemEntity { Id = 1, Title = "Task 1", Category = "Work", Difficulty = 1, IsDone = true },
                    new TodoItemEntity { Id = 2, Title = "Task 2", Category = "Personal", Difficulty = 2, IsDone = true },
                    new TodoItemEntity { Id = 3, Title = "Task 3", Category = "Work", Difficulty = 3, IsDone = true },
                    new TodoItemEntity { Id = 4, Title = "Task 4", Category = "Personal", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Id = 5, Title = "Task 5", Category = "Work", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Id = 6, Title = "Task 6", Category = "Personal", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Id = 7,  Title = "Task 7", Category = "Work", Difficulty = 2, IsDone = false },
                    new TodoItemEntity { Id = 8,  Title = "Task 8", Category = "Personal", Difficulty = 3, IsDone = false },
                    new TodoItemEntity { Id = 9,  Title = "Task 9", Category = "Work", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Id = 10,  Title = "Task 10", Category = "Personal", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Id = 11, Title = "Task 11", Category = "Work", Difficulty = 1, IsDone = true },
                    new TodoItemEntity { Id = 12, Title = "Task 12", Category = "Personal", Difficulty = 2, IsDone = true },
                    new TodoItemEntity { Id = 13, Title = "Task 13", Category = "Work", Difficulty = 3, IsDone = true },
                    new TodoItemEntity { Id = 14, Title = "Task 14", Category = "Personal", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Id = 15, Title = "Task 15", Category = "Work", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Id = 16, Title = "Task 16", Category = "Personal", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Id = 17,  Title = "Task 17", Category = "Work", Difficulty = 2, IsDone = false },
                    new TodoItemEntity { Id = 18,  Title = "Task 18", Category = "Personal", Difficulty = 3, IsDone = false },
                    new TodoItemEntity { Id = 19,  Title = "Task 19", Category = "Work", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Id = 20,  Title = "Task 20", Category = "Personal", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Id = 21, Title = "Task 21", Category = "Work", Difficulty = 1, IsDone = true },
                    new TodoItemEntity { Id = 22, Title = "Task 22", Category = "Personal", Difficulty = 2, IsDone = true },
                    new TodoItemEntity { Id = 23, Title = "Task 23", Category = "Work", Difficulty = 3, IsDone = true },
                    new TodoItemEntity { Id = 24, Title = "Task 24", Category = "Personal", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Id = 25, Title = "Task 25", Category = "Work", Difficulty = 5, IsDone = false },
                    new TodoItemEntity { Id = 26, Title = "Task 26", Category = "Personal", Difficulty = 1, IsDone = false },
                    new TodoItemEntity { Id = 27,  Title = "Task 27", Category = "Work", Difficulty = 2, IsDone = false },
                    new TodoItemEntity { Id = 28,  Title = "Task 28", Category = "Personal", Difficulty = 3, IsDone = false },
                    new TodoItemEntity { Id = 29,  Title = "Task 29", Category = "Work", Difficulty = 4, IsDone = false },
                    new TodoItemEntity { Id = 30,  Title = "Task 30", Category = "Personal", Difficulty = 5, IsDone = false },
                };

                _dbContext.ToDoItems.AddRange(initialTodos);
                _dbContext.SaveChanges();
            }
        }
    }
}
