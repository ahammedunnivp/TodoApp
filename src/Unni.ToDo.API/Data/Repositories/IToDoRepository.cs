using Unni.ToDo.API.Data.Models;
using Unni.ToDo.API.DTOs;

namespace Unni.ToDo.API.Data.Repositories
{
    public interface ITodoRepository
    {
        TodoItemEntity GetById(int id);
        (IEnumerable<TodoItemEntity>, int) Search(Pagination pagination, ToDoFilter filter);
        TodoItemEntity Add(TodoItemEntity item);
        TodoItemEntity Update(TodoItemEntity item);
        void Delete(int id);
    }
}
