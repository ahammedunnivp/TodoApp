using AutoMapper;
using Unni.ToDo.Common.DTOs;
using Unni.ToDo.Common.Models;

namespace Unni.ToDo.API.Mappers.Profiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<TodoItemEntity, TodoItemDto>();
            CreateMap<TodoItemDto, TodoItemEntity>();
            CreateMap<CreateTodoRequest, TodoItemEntity>();
            CreateMap<CreateTodoRequest, TodoItemDto>();

            CreateMap<CategoryEntity, CategoryDto>();
            CreateMap<CategoryDto, CategoryEntity>();
            CreateMap<AddCategoryRequest, CategoryEntity>();
            CreateMap<AddCategoryRequest, CategoryDto>();
        }
    }
}
