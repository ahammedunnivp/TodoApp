using AutoMapper;
using Unni.ToDo.Core.DTOs;
using Unni.ToDo.Core.Models;

namespace Unni.ToDo.API.Mappers.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>();
            CreateMap<CategoryDto, CategoryEntity>();
            CreateMap<AddCategoryRequest, CategoryEntity>();
            CreateMap<AddCategoryRequest, CategoryDto>();
        }
    }
}
