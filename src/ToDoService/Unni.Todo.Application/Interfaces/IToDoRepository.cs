using Unni.Todo.Application.DTOs;
using Unni.Todo.Domain.Entities;

namespace Unni.Todo.Application.Interfaces
{
    public interface ITodoRepository
    {
        TodoItemEntity? GetById(int id);
        (IEnumerable<TodoItemEntity>, int) Search(Pagination pagination, ToDoFilter? filter);
        TodoItemEntity Add(TodoItemEntity item);
        TodoItemEntity Update(TodoItemEntity item);
        void Delete(int id);
    }
}
