using AutoMapper;
using Unni.ToDo.Core.DTOs;
using Unni.ToDo.Core.Models;

namespace Unni.Todo.WebAPI.Mappers.Profiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<TodoItemEntity, TodoItemDto>();
            CreateMap<TodoItemDto, TodoItemEntity>();
            CreateMap<CreateTodoRequest, TodoItemEntity>();
            CreateMap<CreateTodoRequest, TodoItemDto>();
        }
    }
}
