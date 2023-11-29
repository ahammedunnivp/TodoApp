using AutoMapper;
using Unni.ToDo.API.DTOs;
using Unni.ToDo.API.Data.Models;

namespace Unni.ToDo.API.DTOs.Mappers.Profiles
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
