using Unni.ToDo.API.DTOs;

namespace Unni.ToDo.API.Services
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
