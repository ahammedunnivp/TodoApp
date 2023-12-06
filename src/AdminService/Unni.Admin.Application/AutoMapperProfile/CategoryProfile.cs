using AutoMapper;
using Unni.Admin.Application.DTOs;
using Unni.Admin.Domain.Entities;

namespace Unni.Admin.Application.AutoMapperProfile
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
