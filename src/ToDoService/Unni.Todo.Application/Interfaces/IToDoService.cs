using Unni.Todo.Application.DTOs;

namespace Unni.Todo.Application.Interfaces
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
