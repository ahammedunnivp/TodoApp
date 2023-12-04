using Microsoft.Extensions.Logging;
using Unni.Todo.Application.DTOs;
using Unni.Todo.Application.Interfaces;
using Unni.Todo.Domain.Entities;
using Unni.Todo.Infrastructure.Context;

namespace Unni.Todo.Infrastructure.Repositories
{
    public class TodoItemRepository : ITodoRepository
    {
        private readonly ToDoDBContext _dbContext;
        private readonly ILogger _logger;

        public TodoItemRepository(ToDoDBContext dbContext, ILogger<TodoItemRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public TodoItemEntity Add(TodoItemEntity item)
        {
            _logger.LogInformation("A new todo item adding to DB");
            _dbContext.ToDoItems.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            var todoItem = _dbContext.ToDoItems.Find(id);
            if (todoItem != null)
            {
                _logger.LogInformation("Deleting the todo item with Id: {id}", id);
                _dbContext.ToDoItems.Remove(todoItem);
            }
        }

        public (IEnumerable<TodoItemEntity>, int) Search(Pagination pagination, ToDoFilter? filter)
        {
            var query = _dbContext.ToDoItems.AsQueryable();


            if (filter != null)
            {
                if (filter.IsDoneFilter.HasValue)
                {
                    query = query.Where(todo => todo.IsDone == filter.IsDoneFilter.Value);
                }
                if (filter.Difficulty.HasValue)
                {
                    query = query.Where(todo => todo.Difficulty == filter.Difficulty.Value);
                }
                if (!string.IsNullOrEmpty(filter.Category))
                {
                    query = query.Where(todo => todo.Category == filter.Category);
                }

            }
            var totalCount = query.Count();

            switch (pagination.SortField)
            {
                case "Title":
                    query = pagination.IsSortAscending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title);
                    break;
                case "Category":
                    query = pagination.IsSortAscending ? query.OrderBy(t => t.Category) : query.OrderByDescending(t => t.Category);
                    break;
                case "Difficulty":
                    query = pagination.IsSortAscending ? query.OrderBy(t => t.Difficulty) : query.OrderByDescending(t => t.Difficulty);
                    break;
                case "IsDone":
                    query = pagination.IsSortAscending ? query.OrderBy(t => t.IsDone) : query.OrderByDescending(t => t.IsDone);
                    break;
                default:
                    query = pagination.IsSortAscending ? query.OrderBy(t => t.Id) : query.OrderByDescending(t => t.Id);
                    break;
            }
            query = query.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);

            _logger.LogInformation("Fething todo items from DB");
            var todoItems = query.ToList();
            return (todoItems, totalCount);
        }

        public TodoItemEntity GetById(int id)
        {
            _logger.LogInformation("Fething todo item with Id: {id}", id);

            var todoItem = _dbContext.ToDoItems.Find(id);
            return todoItem;
        }

        public TodoItemEntity Update(TodoItemEntity item)
        {
            _logger.LogInformation("Updaing todo item with Id: {id}", item.Id);
            _dbContext.ToDoItems.Update(item);
            return item;
        }
    }
}
