using Unni.ToDo.Core.DTOs;
using Unni.ToDo.Core.Models;

namespace Unni.ToDo.Core.Interfaces
{
    public interface ITodoRepository
    {
        TodoItemEntity GetById(int id);
        (IEnumerable<TodoItemEntity>, int) Search(Pagination pagination, ToDoFilter? filter);
        TodoItemEntity Add(TodoItemEntity item);
        TodoItemEntity Update(TodoItemEntity item);
        void Delete(int id);
    }
}
