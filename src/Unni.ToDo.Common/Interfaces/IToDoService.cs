using Unni.ToDo.Common.DTOs;

namespace Unni.ToDo.Common.Interfaces
{
    public interface ITodoService
    {
        TodoItemDto GetById(int id);
        PaginatedResponseDto<TodoItemDto> Search(GetTodoRequest request);
        TodoItemDto AddToDoItem(CreateTodoRequest item);
        TodoItemDto UpdateToDoItem(int id, TodoItemDto item);
        void DeleteToDoItemById(int id);
    }
}
