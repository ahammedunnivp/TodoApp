using AutoMapper;
using Unni.Todo.Application.DTOs;
using Unni.Todo.Domain.Entities;

namespace Unni.Todo.Application.AutoMapperProfile
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
