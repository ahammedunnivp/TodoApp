using Unni.ToDo.Common.DTOs;
using Unni.ToDo.Common.Models;

namespace Unni.ToDo.Common.Interfaces
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
